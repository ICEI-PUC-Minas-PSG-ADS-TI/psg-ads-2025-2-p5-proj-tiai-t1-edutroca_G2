using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities.ConteudoAggregate;
using EduTroca.UseCases.Common.Authorization.Contexts;
using EduTroca.UseCases.Common.Authorization.Requirements;

namespace EduTroca.UseCases.Common.Authorization.Policies;
public class ContentAuthorizationPolicies
{
    public static IsOwnerRequirement<ContentModificationContext> IsOwner { get; }
    = new();
    public static IsAdminRequirement<ContentModificationContext> IsAdmin { get; }
    = new();
    public static CanModifyOwnRequirement CreateCanModifyOwn(
        IRepository<Conteudo> repository)
        => new(repository);
}
