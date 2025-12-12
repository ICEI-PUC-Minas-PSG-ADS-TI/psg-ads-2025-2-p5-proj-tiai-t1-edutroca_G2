using EduTroca.UseCases.Common.Authorization.Contexts;
using ErrorOr;

namespace EduTroca.UseCases.Common.Authorization.Policies;
public interface IAuthorizationRequirement
{
    Task<ErrorOr<Success>> EvaluateAsync(
        AuthorizationContext context,
        CancellationToken cancellationToken);
}
