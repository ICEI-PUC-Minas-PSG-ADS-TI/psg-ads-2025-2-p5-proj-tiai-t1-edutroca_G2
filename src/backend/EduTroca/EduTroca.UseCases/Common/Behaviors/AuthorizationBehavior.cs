using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Common.Behaviors;
public class AuthorizationBehavior<TRequest, TResponse>(IAuthorizer<TRequest>? authorizer = null)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    private readonly IAuthorizer<TRequest>? _authorizer = authorizer;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_authorizer is null)
            return await next();
        var result = await authorizer!.AuthorizeAsync(request, cancellationToken);

        if (result.IsError)
            return (dynamic)Error.Forbidden("Role.Unauthorized", "Usuario não possui autorização para executar este comando.");

        return await next();
    }
}
