using EduTroca.UseCases.Common.Authorization.Contexts;
using ErrorOr;

namespace EduTroca.UseCases.Common.Authorization.Requirements;
public class CanModifySelfRequirement : IAuthorizationRequirement<UserModificationContext>
{
    public Task<ErrorOr<Success>> EvaluateAsync(
        UserModificationContext context,
        CancellationToken cancellationToken)
    {
        if (context.TargetUserId == context.CurrentUserId)
            return Task.FromResult<ErrorOr<Success>>(Result.Success);

        return Task.FromResult<ErrorOr<Success>>(
            Error.Forbidden("Authorization.NotSelf",
                "Usuário só pode modificar a si mesmo."));
    }
}
