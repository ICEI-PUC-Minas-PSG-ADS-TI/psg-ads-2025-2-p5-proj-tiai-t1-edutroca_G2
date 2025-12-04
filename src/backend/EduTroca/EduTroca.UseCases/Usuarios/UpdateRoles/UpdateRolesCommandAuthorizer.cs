using EduTroca.UseCases.Common.Guards;
using ErrorOr;

namespace EduTroca.UseCases.Usuarios.UpdateRoles;
public class UpdateRolesCommandAuthorizer(HierarchyGuard hierarchyGuard) : IAuthorizer<UpdateRolesCommand>
{
    private readonly HierarchyGuard _hierarchyGuard = hierarchyGuard;
    public async Task<ErrorOr<Success>> AuthorizeAsync(UpdateRolesCommand request, CancellationToken cancellationToken)
    {
        return await _hierarchyGuard.ValidateUserModificationAsync(request.usuarioId);
    }
}
