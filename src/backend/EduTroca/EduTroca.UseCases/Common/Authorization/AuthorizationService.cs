using EduTroca.UseCases.Common.Authorization.Contexts;
using ErrorOr;

namespace EduTroca.UseCases.Common.Authorization;
public class AuthorizationService : IAuthorizationService
{
    public async Task<ErrorOr<Success>> AuthorizeAsync<TContext>(
        IAuthorizationRequirement<TContext> requirement,
        TContext context,
        CancellationToken cancellationToken = default)
        where TContext : AuthorizationContext
    {
        return await requirement.EvaluateAsync(context, cancellationToken);
    }

    public async Task<ErrorOr<Success>> AuthorizeAsync<TContext>(
        IEnumerable<IAuthorizationRequirement<TContext>> requirements,
        TContext context,
        CancellationToken cancellationToken = default)
        where TContext : AuthorizationContext
    {
        foreach (var requirement in requirements)
        {
            var result = await requirement.EvaluateAsync(context, cancellationToken);

            if (!result.IsError)
                return Result.Success;

            if (result.Errors.Any(x => x.Type != ErrorType.Forbidden && x.Type != ErrorType.Unauthorized))
                return result;
        }

        return Error.Forbidden("Authorization.Denied",
            "Nenhum requisito de autorização foi satisfeito.");
    }
}
