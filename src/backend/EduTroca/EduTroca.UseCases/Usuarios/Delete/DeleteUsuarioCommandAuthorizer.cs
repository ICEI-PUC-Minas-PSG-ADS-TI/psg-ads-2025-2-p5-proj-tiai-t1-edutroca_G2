using EduTroca.UseCases.Common.Guards;
using ErrorOr;

namespace EduTroca.UseCases.Usuarios.Delete;
public class DeleteUsuarioCommandAuthorizer(HierarchyGuard hierarchyGuard)
    : IAuthorizer<DeleteUsuarioCommand>
{
    private readonly HierarchyGuard _hierarchyGuard = hierarchyGuard;
    public async Task<ErrorOr<Success>> AuthorizeAsync(DeleteUsuarioCommand request, CancellationToken cancellationToken)
    {
        return await _hierarchyGuard.ValidateUserModificationAsync(request.usuarioId);
    }
}
