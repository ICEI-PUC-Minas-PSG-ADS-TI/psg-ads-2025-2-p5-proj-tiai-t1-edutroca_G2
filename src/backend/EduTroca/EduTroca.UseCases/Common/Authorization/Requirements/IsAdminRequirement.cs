using EduTroca.Core.Enums;
using EduTroca.UseCases.Common.Authorization.Contexts;
using ErrorOr;

namespace EduTroca.UseCases.Common.Authorization.Requirements;
public class IsAdminRequirement<TContext> : IAuthorizationRequirement<TContext>
    where TContext : AuthorizationContext
{
    public Task<ErrorOr<Success>> EvaluateAsync(
        TContext context,
        CancellationToken cancellationToken)
    {
        if (context.IsInRole(ERole.Admin))
            return Task.FromResult<ErrorOr<Success>>(Result.Success);

        return Task.FromResult<ErrorOr<Success>>(
            Error.Forbidden("Authorization.NotAdmin",
                "Apenas Admins pode executar esta ação."));
    }
}
