using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Specifications;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Usuarios.UpdateProfile;
public class UpdateProfileCommandHandler(IRepository<Usuario> usuarioRepository, IFileService fileService)
    : IRequestHandler<UpdateProfileCommand, ErrorOr<Success>>
{
    private readonly IRepository<Usuario> _usuarioRepository = usuarioRepository;
    private readonly IFileService _fileService = fileService;
    private const string FolderName = "Imagens";

    public async Task<ErrorOr<Success>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var usuarioByIdSpecification = new UsuarioById(request.usuarioId);
        var usuario = await _usuarioRepository.FirstOrDefaultAsync(usuarioByIdSpecification);

        if (usuario is null)
            return Error.NotFound("Usuario.NotFound", "Usuario inexistente ou inativo.");

        if (request.DeletePicture)
        {
            if (!string.IsNullOrEmpty(usuario.CaminhoImagem))
                await _fileService.RemoveFileAsync(usuario.CaminhoImagem, cancellationToken);
            usuario.UpdatePicture(string.Empty);
        }
        else if (request.picture is not null)
        {
            if (!string.IsNullOrEmpty(usuario.CaminhoImagem))
                await _fileService.RemoveFileAsync(usuario.CaminhoImagem, cancellationToken);
            var extension = GetExtensionFromMimeType(request.picture.ContentType);
            var newFileName = $"{usuario.Id}{extension}";
            await _fileService.SaveFileAsync(FolderName, newFileName, request.picture.Stream, cancellationToken);
            usuario.UpdatePicture($"{FolderName}/{newFileName}");
        }
        usuario.UpdateProfile(request.nome, request.bio);

        await _usuarioRepository.UpdateAsync(usuario);
        await _usuarioRepository.SaveChangesAsync();

        return Result.Success;
    }
    private string GetExtensionFromMimeType(string contentType)
    {
        return contentType switch
        {
            "image/png" => ".png",
            "image/gif" => ".gif",
            "image/jpeg" => ".jpg",
            "image/jpg" => ".jpg",
            "image/webp" => ".webp",
            _ => ".jpg"
        };
    }
}
