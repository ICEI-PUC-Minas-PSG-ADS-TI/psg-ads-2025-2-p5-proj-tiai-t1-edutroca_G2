using EduTroca.Core.Enums;

namespace EduTroca.Core.Abstractions;
public interface ICurrentUserService
{
    Guid UserId { get; }
    bool IsInRole(ERole role);
}
