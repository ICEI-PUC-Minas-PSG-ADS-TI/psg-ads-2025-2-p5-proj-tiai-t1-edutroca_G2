using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.UseCases.Common.Authorization.Contexts;
using EduTroca.UseCases.Common.Authorization.Requirements;

namespace EduTroca.UseCases.Common.Authorization.Policies;
public static class UserAuthorizationPolicies
{
    public static IsOwnerRequirement<UserModificationContext> IsOwner { get; }
        = new();

    public static CanModifySelfRequirement CanModifySelf { get; }
        = new();

    public static CanModifyLowerRoleRequirement CreateCanModifyLowerRole(
        IRepository<Usuario> repository)
        => new(repository);
}
