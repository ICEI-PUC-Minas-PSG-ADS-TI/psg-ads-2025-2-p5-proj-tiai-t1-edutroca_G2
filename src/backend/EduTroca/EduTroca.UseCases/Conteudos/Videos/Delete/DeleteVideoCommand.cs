using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Conteudos.Videos.Delete;
public record DeleteVideoCommand(Guid videoId) : IRequest<ErrorOr<Success>>;
