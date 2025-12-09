using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Conteudos.Like;
public record ToggleLikeCommand(Guid conteudoId) : IRequest<ErrorOr<Success>>;
