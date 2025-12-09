using EduTroca.Core.Enums;
using EduTroca.UseCases.Usuarios.DTOs;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Usuarios.UpdateRoles;
public record UpdateRolesCommand(Guid usuarioId, List<ERole> rolesCodes) : IRequest<ErrorOr<UsuarioDTO>>;
