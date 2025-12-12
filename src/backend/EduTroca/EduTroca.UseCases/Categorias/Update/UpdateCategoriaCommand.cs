using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Categorias.Update;
public record UpdateCategoriaCommand(Guid id, string nome, string descricao) : IRequest<ErrorOr<Success>>;
