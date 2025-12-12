using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Specifications;
using EduTroca.UseCases.Common.Helpers;
using EduTroca.UseCases.Usuarios.DTOs;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Usuarios.UpdateProfile;
public class UpdateProfileCommandHandler(IRepository<Usuario> usuarioRepository, IFileService fileService)
    : IRequestHandler<UpdateProfileCommand, ErrorOr<UsuarioDTO>>
{
    private readonly IRepository<Usuario> _usuarioRepository = usuarioRepository;
    private readonly IFileService _fileService = fileService;
    private const string FolderName = "Imagens";

    public async Task<ErrorOr<UsuarioDTO>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
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
            var extension = FileHelpers.GetExtensionFromMimeType(request.picture.ContentType);
            var newFileName = $"{usuario.Id}{extension}";
            await _fileService.SaveFileAsync(FolderName, newFileName, request.picture.Stream, cancellationToken);
            usuario.UpdatePicture($"{FolderName}/{newFileName}");
        }
        usuario.UpdateProfile(request.nome, request.bio);

        await _usuarioRepository.UpdateAsync(usuario);
        await _usuarioRepository.SaveChangesAsync();

        return UsuarioDTO.FromUsuario(usuario);
    }
}
