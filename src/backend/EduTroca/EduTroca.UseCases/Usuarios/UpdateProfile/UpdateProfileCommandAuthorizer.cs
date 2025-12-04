using EduTroca.UseCases.Common.Guards;
using ErrorOr;

namespace EduTroca.UseCases.Usuarios.UpdateProfile;
public class UpdateProfileCommandAuthorizer(HierarchyGuard hierarchyGuard) : IAuthorizer<UpdateProfileCommand>
{
    private readonly HierarchyGuard _hierarchyGuard = hierarchyGuard;
    public async Task<ErrorOr<Success>> AuthorizeAsync(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        return await _hierarchyGuard.ValidateUserModificationAsync(request.usuarioId);
    }
}