using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Conteudos.Comment;
public record CommentCommand(Guid conteudoId, string texto) : IRequest<ErrorOr<Success>>;
