using EduTroca.Core.Enums;

namespace EduTroca.UseCases.Common.Authorization.Contexts;
public abstract class AuthorizationContext
{
    public Guid CurrentUserId { get; private set; }
    private readonly Func<ERole, bool> _isInRole;

    protected AuthorizationContext(Guid currentUserId, Func<ERole, bool> isInRole)
    {
        CurrentUserId = currentUserId;
        _isInRole = isInRole;
    }
    public bool IsInRole(ERole role) => _isInRole(role);
    public bool HasAnyRole(params ERole[] roles) => roles.Any(IsInRole);
    public bool HasAllRoles(params ERole[] roles) => roles.All(IsInRole);
}
