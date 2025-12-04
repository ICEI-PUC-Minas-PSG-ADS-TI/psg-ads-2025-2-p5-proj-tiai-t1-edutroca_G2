using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Usuarios.Delete;
public record DeleteUsuarioCommand(Guid usuarioId) : IRequest<ErrorOr<Success>>;
