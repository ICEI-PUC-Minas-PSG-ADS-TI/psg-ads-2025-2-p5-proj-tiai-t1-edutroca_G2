using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Conteudos.Perguntas.Delete;
public record DeletePerguntaCommand(Guid perguntaId) : IRequest<ErrorOr<Success>>;
