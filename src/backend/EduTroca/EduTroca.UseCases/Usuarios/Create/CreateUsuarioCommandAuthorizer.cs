using EduTroca.Core.Abstractions;
using EduTroca.Core.Enums;
using ErrorOr;

namespace EduTroca.UseCases.Usuarios.Create;
public class CreateUsuarioCommandAuthorizer(
    ICurrentUserService currentUser)
    : IAuthorizer<CreateUsuarioCommand>
{
    private readonly ICurrentUserService _currentUser = currentUser;
    public async Task<ErrorOr<Success>> AuthorizeAsync(CreateUsuarioCommand request, CancellationToken cancellationToken)
    {
        if (request.rolesCodes is not null && request.rolesCodes.Count > 0)
        {
            var canAssignRoles = _currentUser.IsInRole(ERole.Admin) || _currentUser.IsInRole(ERole.Owner);
            if (!canAssignRoles)
                return Error.Forbidden(description: "Você não tem permissão para definir cargos.");
        }
        return Result.Success;
    }
}
