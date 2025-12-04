using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Enums;
using EduTroca.Core.Specifications;
using ErrorOr;

namespace EduTroca.UseCases.Common.Guards;
public class HierarchyGuard(
    ICurrentUserService currentUser,
    IRepository<Usuario> usuarioRepository)
{
    private readonly ICurrentUserService _currentUser = currentUser;
    private readonly IRepository<Usuario> _usuarioRepository = usuarioRepository;

    public async Task<ErrorOr<Success>> ValidateUserModificationAsync(Guid targetUserId)
    {
        var currentUserId = _currentUser.UserId;
        var isOwner = _currentUser.IsInRole(ERole.Owner);
        var isAdmin = _currentUser.IsInRole(ERole.Admin);
        var isSelf = currentUserId == targetUserId;

        if (!isAdmin && !isOwner && !isSelf) return Error.Forbidden();

        if (isOwner || isSelf) return Result.Success;

        var targetUserSpec = new UsuarioById(targetUserId);
        var targetUser = await _usuarioRepository.FirstOrDefaultAsync(targetUserSpec);

        if (targetUser is null) return Result.Success;

        var targetIsAdmin = targetUser.Roles.Any(r => r.Id == (int)ERole.Admin);
        var targetIsOwner = targetUser.Roles.Any(r => r.Id == (int)ERole.Owner);

        if (targetIsAdmin || targetIsOwner)
            return Error.Forbidden();

        return Result.Success;
    }
}
