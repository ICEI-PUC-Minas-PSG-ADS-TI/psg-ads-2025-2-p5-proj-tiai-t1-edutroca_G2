using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Conteudos.Perguntas.Update;
public record UpdatePerguntaCommand(Guid perguntaId, string titulo, string descricao, Guid categoriaId, string texto) 
    : IRequest<ErrorOr<Success>>;
