using EduTroca.Core.Enums;

namespace EduTroca.UseCases.Common.Authorization.Contexts;
public class UserModificationContext : AuthorizationContext
{
    public Guid TargetUserId { get; private set; }

    public UserModificationContext(Guid currentUserId, Func<ERole, bool> isInRole,
        Guid targetUserId)
        : base(currentUserId, isInRole)
    {
        TargetUserId = targetUserId;
    }
}
