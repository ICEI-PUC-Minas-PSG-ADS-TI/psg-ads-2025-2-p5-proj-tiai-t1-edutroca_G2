using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities.ConteudoAggregate;
using EduTroca.UseCases.Common.Authorization;
using EduTroca.UseCases.Common.Authorization.Contexts;
using EduTroca.UseCases.Common.Authorization.Policies;
using ErrorOr;

namespace EduTroca.UseCases.Conteudos.Perguntas.Delete;
public class DeletePerguntaCommandAuthorizer(
    ICurrentUserService currentUser,
    IAuthorizationService authService,
    IRepository<Conteudo> conteudoRepository)
    : IAuthorizer<DeletePerguntaCommand>
{
    private readonly ICurrentUserService _currentUser = currentUser;
    private readonly IAuthorizationService _authService = authService;
    private readonly IRepository<Conteudo> _conteudoRepository = conteudoRepository;
    public async Task<ErrorOr<Success>> AuthorizeAsync(
        DeletePerguntaCommand request,
        CancellationToken cancellationToken)
    {
        var context = new ContentModificationContext(_currentUser.UserId, _currentUser.IsInRole, request.perguntaId);

        var requirements = new IAuthorizationRequirement<ContentModificationContext>[]
        {
            ContentAuthorizationPolicies.IsOwner,
            ContentAuthorizationPolicies.IsAdmin,
            ContentAuthorizationPolicies.CreateCanModifyOwn(_conteudoRepository)
        };

        var result = await _authService.AuthorizeAsync(
            requirements, context, cancellationToken);

        return result;
    }
}
