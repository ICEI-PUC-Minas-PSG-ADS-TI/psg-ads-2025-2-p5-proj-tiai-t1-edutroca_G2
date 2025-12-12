using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Categorias.Delete;
public record DeleteCategoriaCommand(Guid id) : IRequest<ErrorOr<Success>>;
