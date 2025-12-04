using EduTroca.UseCases.Common.Guards;
using ErrorOr;

namespace EduTroca.UseCases.Usuarios.UpdateSenha;
public class UpdateSenhaCommandAuthorizer(HierarchyGuard hierarchyGuard) : IAuthorizer<UpdateSenhaCommand>
{
    private readonly HierarchyGuard _hierarchyGuard = hierarchyGuard;
    public async Task<ErrorOr<Success>> AuthorizeAsync(UpdateSenhaCommand request, CancellationToken cancellationToken)
    {
        return await _hierarchyGuard.ValidateUserModificationAsync(request.usuarioId);
    }
}
