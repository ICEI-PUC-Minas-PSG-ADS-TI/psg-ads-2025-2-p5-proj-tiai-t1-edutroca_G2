using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities;
using EduTroca.Core.Entities.ConteudoAggregate;
using EduTroca.Core.Specifications;
using EduTroca.UseCases.Common.Helpers;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Conteudos.Videos.Update;
public class UpdateVideoCommandHandler(
    IRepository<Video> videoRepository,
    IRepository<Categoria> categoriaRepository,
    IFileService fileService)
    : IRequestHandler<UpdateVideoCommand, ErrorOr<Success>>
{
    private readonly IRepository<Video> _videoRepository = videoRepository;
    private readonly IRepository<Categoria> _categoriaRepository = categoriaRepository;
    private readonly IFileService _fileService = fileService;
    private const string ImageFolder = "Capas";
    public async Task<ErrorOr<Success>> Handle(UpdateVideoCommand request, CancellationToken cancellationToken)
    {
        var videoByIdSpec = new VideoById(request.videoId);
        var video = await _videoRepository.FirstOrDefaultAsync(videoByIdSpec);
        if (video is null)
            return Error.NotFound("Video.NotFound", "Video inexistente.");

        var categoriaByIdSpec = new CategoriaById(request.categoriaId);
        var categoriaExists = await _categoriaRepository.AnyAsync(categoriaByIdSpec);
        if (!categoriaExists)
            return Error.NotFound("Categoria.NotFound", "Categoria inexistente.");

        var imageExt = FileHelpers.GetExtensionFromMimeType(request.imagem.ContentType);
        var imageFileName = $"{Guid.NewGuid()}{imageExt}";
        await _fileService.RemoveFileAsync(video.CaminhoImagem, cancellationToken);
        await _fileService.SaveFileAsync(ImageFolder, imageFileName, request.imagem.Stream, cancellationToken);

        var pathImagemFinal = $"{ImageFolder}/{imageFileName}";

        video.Update(request.titulo, request.descricao, request.categoriaId, pathImagemFinal);
        await _videoRepository.UpdateAsync(video);
        await _videoRepository.SaveChangesAsync();
        return Result.Success;
    }
}
