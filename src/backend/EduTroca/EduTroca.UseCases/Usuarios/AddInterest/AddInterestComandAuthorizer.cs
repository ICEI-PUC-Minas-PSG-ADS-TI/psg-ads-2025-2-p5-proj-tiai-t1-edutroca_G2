using EduTroca.UseCases.Common.Guards;
using ErrorOr;

namespace EduTroca.UseCases.Usuarios.AddInterest;
public class AddInterestComandAuthorizer(HierarchyGuard hierarchyGuard) : IAuthorizer<AddInterestCommand>
{
    private readonly HierarchyGuard _hierarchyGuard = hierarchyGuard;
    public async Task<ErrorOr<Success>> AuthorizeAsync(AddInterestCommand request, CancellationToken cancellationToken)
    {
        return await _hierarchyGuard.ValidateUserModificationAsync(request.usuarioId);
    }
}
