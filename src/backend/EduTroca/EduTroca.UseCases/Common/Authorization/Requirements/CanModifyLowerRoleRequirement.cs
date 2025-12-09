using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Enums;
using EduTroca.Core.Specifications;
using EduTroca.UseCases.Common.Authorization.Contexts;
using ErrorOr;

namespace EduTroca.UseCases.Common.Authorization.Requirements;
public class CanModifyLowerRoleRequirement(IRepository<Usuario> usuarioRepository)
    : IAuthorizationRequirement<UserModificationContext>
{
    public async Task<ErrorOr<Success>> EvaluateAsync(
        UserModificationContext context,
        CancellationToken cancellationToken)
    {
        var isAdmin = context.IsInRole(ERole.Admin);
        var isOwner = context.IsInRole(ERole.Owner);

        if (!isAdmin && !isOwner)
            return Error.Forbidden("Authorization.InsufficientRole",
                "Usuário não possui cargo suficiente.");

        if (isOwner)
            return Result.Success;

        var spec = new UsuarioById(context.TargetUserId, includeDetails: true);
        var targetUsuario = await usuarioRepository
            .FirstOrDefaultAsync(spec, cancellationToken);

        if (targetUsuario is null)
            return Error.NotFound("Usuario.NotFound", "Usuario inexistente ou inativo.");

        var targetIsAdminOrOwner = targetUsuario.Roles
            .Any(r => r.Code is ERole.Admin or ERole.Owner);

        if (targetIsAdminOrOwner)
            return Error.Forbidden("Authorization.HierarchyViolation",
                "Admin não pode modificar outros Admins ou Owners.");

        return Result.Success;
    }
}
