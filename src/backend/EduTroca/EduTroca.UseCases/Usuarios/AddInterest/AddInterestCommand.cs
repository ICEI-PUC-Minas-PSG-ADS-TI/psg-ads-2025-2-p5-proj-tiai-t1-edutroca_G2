using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Usuarios.AddInterest;
public record AddInterestCommand(Guid usuarioId, Guid categoriaId) : IRequest<ErrorOr<Success>>;
