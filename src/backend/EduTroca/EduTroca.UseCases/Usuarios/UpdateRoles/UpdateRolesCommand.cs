using EduTroca.Core.Enums;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Usuarios.UpdateRoles;
public record UpdateRolesCommand(Guid usuarioId, List<ERole> rolesIds) : IRequest<ErrorOr<Success>>;
