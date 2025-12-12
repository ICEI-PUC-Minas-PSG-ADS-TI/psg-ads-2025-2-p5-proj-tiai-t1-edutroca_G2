using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Usuarios.SetInterests;
public record SetInterestsCommand(Guid usuarioId, List<Guid> categoriasIds) : IRequest<ErrorOr<Success>>;
