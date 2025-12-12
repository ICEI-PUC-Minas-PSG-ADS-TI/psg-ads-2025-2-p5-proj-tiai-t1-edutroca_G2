using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities.ConteudoAggregate;
using EduTroca.Core.Specifications;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Conteudos.Videos.Delete;
public class DeleteVideoCommandHandler(IRepository<Video> videoRepository, IFileService fileService)
    : IRequestHandler<DeleteVideoCommand, ErrorOr<Success>>
{
    private readonly IRepository<Video> _videoRepository = videoRepository;
    private readonly IFileService _fileService = fileService;
    public async Task<ErrorOr<Success>> Handle(DeleteVideoCommand request, CancellationToken cancellationToken)
    {
        var videoByIdSpec = new VideoById(request.videoId);
        var video = await _videoRepository.FirstOrDefaultAsync(videoByIdSpec, cancellationToken);
        if(video is null)
            return Error.NotFound("Video.NotFound", "Video inexistente.");
        await _fileService.RemoveFileAsync(video.CaminhoImagem, cancellationToken);
        await _fileService.RemoveFileAsync(video.CaminhoVideo, cancellationToken);
        await _videoRepository.RemoveAsync(video);
        await _videoRepository.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
