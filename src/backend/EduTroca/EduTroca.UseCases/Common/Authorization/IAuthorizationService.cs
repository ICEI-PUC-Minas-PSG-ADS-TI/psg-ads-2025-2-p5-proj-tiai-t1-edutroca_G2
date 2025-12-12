using EduTroca.UseCases.Common.Authorization.Contexts;
using ErrorOr;

namespace EduTroca.UseCases.Common.Authorization;
public interface IAuthorizationService
{
    Task<ErrorOr<Success>> AuthorizeAsync<TContext>(
        IAuthorizationRequirement<TContext> requirement,
        TContext context,
        CancellationToken cancellationToken = default)
        where TContext : AuthorizationContext;

    Task<ErrorOr<Success>> AuthorizeAsync<TContext>(
        IEnumerable<IAuthorizationRequirement<TContext>> requirements,
        TContext context,
        CancellationToken cancellationToken = default)
        where TContext : AuthorizationContext;
}
