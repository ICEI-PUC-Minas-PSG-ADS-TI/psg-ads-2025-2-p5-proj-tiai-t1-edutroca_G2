using EduTroca.Core.Enums;

namespace EduTroca.UseCases.Common.Authorization.Contexts;
public class ContentModificationContext : AuthorizationContext
{
    public Guid ContentId { get; private set; }

    public ContentModificationContext(Guid currentUserId, Func<ERole, bool> isInRole,
        Guid contentId)
        : base(currentUserId, isInRole)
    {
        ContentId = contentId;
    }
}
