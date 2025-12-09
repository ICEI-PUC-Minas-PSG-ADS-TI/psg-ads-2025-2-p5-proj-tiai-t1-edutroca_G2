using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.UseCases.Common.Authorization;
using EduTroca.UseCases.Common.Authorization.Contexts;
using EduTroca.UseCases.Common.Authorization.Policies;
using ErrorOr;

namespace EduTroca.UseCases.Usuarios.UpdateRoles;
public class UpdateRolesCommandAuthorizer(
    ICurrentUserService currentUser,
    IAuthorizationService authService,
    IRepository<Usuario> usuarioRepository) : IAuthorizer<UpdateRolesCommand>
{
    private readonly ICurrentUserService _currentUser = currentUser;
    private readonly IAuthorizationService _authService = authService;
    private readonly IRepository<Usuario> _usuarioRepository = usuarioRepository;
    public async Task<ErrorOr<Success>> AuthorizeAsync(UpdateRolesCommand request, CancellationToken cancellationToken)
    {
        var context = new UserModificationContext(_currentUser.UserId, _currentUser.IsInRole, request.usuarioId);

        var requirements = new IAuthorizationRequirement<UserModificationContext>[]
        {
            UserAuthorizationPolicies.IsOwner,
            UserAuthorizationPolicies.CanModifySelf,
            UserAuthorizationPolicies.CreateCanModifyLowerRole(_usuarioRepository)
        };

        var result = await _authService.AuthorizeAsync(
            requirements, context, cancellationToken);

        return result;
    }
}
