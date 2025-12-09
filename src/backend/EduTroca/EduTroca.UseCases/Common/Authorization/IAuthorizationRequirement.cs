using EduTroca.UseCases.Common.Authorization.Contexts;
using ErrorOr;

namespace EduTroca.UseCases.Common.Authorization;
public interface IAuthorizationRequirement<TContext> where TContext : AuthorizationContext
{
    Task<ErrorOr<Success>> EvaluateAsync(
        TContext context,
        CancellationToken cancellationToken);
}
