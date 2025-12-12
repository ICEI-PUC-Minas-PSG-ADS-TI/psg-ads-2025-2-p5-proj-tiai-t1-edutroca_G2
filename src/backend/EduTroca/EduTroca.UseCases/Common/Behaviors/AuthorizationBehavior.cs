using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Common.Behaviors;
public class AuthorizationBehavior<TRequest, TResponse>(
    IAuthorizer<TRequest>? authorizer = null)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (authorizer is null)
            return await next();

        var authResult = await authorizer.AuthorizeAsync(request, cancellationToken);

        if (authResult.IsError)
            return (dynamic)authResult.Errors;

        return await next();
    }
}
