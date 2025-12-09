using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities;
using EduTroca.Core.Entities.ConteudoAggregate;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Specifications;
using EduTroca.UseCases.Common.Helpers;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Conteudos.Videos.Create;
public class CreateVideoCommandHandler(
    IFileService fileService,
    IRepository<Conteudo> conteudoRepository,
    IRepository<Usuario> usuarioRepository,
    IRepository<Categoria> categoriaRepository,
    ICurrentUserService currentUser)
    : IRequestHandler<CreateVideoCommand, ErrorOr<Guid>>
{
    private readonly IFileService _fileService = fileService;
    private readonly IRepository<Conteudo> _conteudoRepository = conteudoRepository;
    private readonly IRepository<Usuario> _usuarioRepository = usuarioRepository;
    private readonly IRepository<Categoria> _categoriaRepository = categoriaRepository;
    private readonly ICurrentUserService _currentUser = currentUser;
    private const string VideoFolder = "Videos";
    private const string ImageFolder = "Capas";

    public async Task<ErrorOr<Guid>> Handle(CreateVideoCommand request, CancellationToken cancellationToken)
    {
        var categoryByIdSpec = new CategoriaById(request.categoriaId);
        var categoriaExists = await _categoriaRepository
            .AnyAsync(categoryByIdSpec, cancellationToken);

        if (!categoriaExists)
            return Error.NotFound("Categoria.NotFound", "Categoria inexistente.");
        var filesId = Guid.NewGuid();

        var videoFileName = $"{filesId}{FileHelpers.GetExtensionFromMimeType(request.video.ContentType)}";
        var imageFileName = $"{filesId}{FileHelpers.GetExtensionFromMimeType(request.imagem.ContentType)}";

        await _fileService.SaveFileAsync(VideoFolder, videoFileName, request.video.Stream, cancellationToken);

        await _fileService.SaveFileAsync(ImageFolder, imageFileName, request.imagem.Stream, cancellationToken);

        var caminhoVideo = $"{VideoFolder}/{videoFileName}";
        var caminhoImagem = $"{ImageFolder}/{imageFileName}";

        var video = new Video(
            request.titulo,
            request.descricao,
            _currentUser.UserId,
            request.categoriaId,
            caminhoVideo,
            caminhoImagem
        );

        var conteudoByAutorSpec = new ConteudoByAutor(_currentUser.UserId);
        var hasConteudo = await _conteudoRepository.AnyAsync(conteudoByAutorSpec, cancellationToken);
        if (!hasConteudo)
        {
            var usuarioByIdSpec = new UsuarioById(_currentUser.UserId);
            var usuario = await _usuarioRepository.FirstOrDefaultAsync(usuarioByIdSpec, cancellationToken);
            usuario.SetNivel(Core.Enums.ENivel.Criador);
            await _usuarioRepository.UpdateAsync(usuario);
        }

        await _conteudoRepository.AddAsync(video, cancellationToken);
        await _conteudoRepository.SaveChangesAsync(cancellationToken);

        return video.Id;
    }
}
