using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Conteudos.Dislike;
public record ToggleDislikeCommand(Guid conteudoId) : IRequest<ErrorOr<Success>>;
