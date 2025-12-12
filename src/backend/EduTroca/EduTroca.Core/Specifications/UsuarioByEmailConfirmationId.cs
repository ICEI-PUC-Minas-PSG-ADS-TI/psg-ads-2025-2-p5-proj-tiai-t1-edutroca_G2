using EduTroca.Core.Entities.UsuarioAggregate;

namespace EduTroca.Core.Specifications;
public class UsuarioByEmailConfirmationId : Specification<Usuario>
{
    public UsuarioByEmailConfirmationId(Guid id, bool includeCredentials = false)
        : base(u => u.EmailConfirmationCode.Id == id)
    {
        AddInclude(u => u.EmailConfirmationCode);
        if (includeCredentials)
            AddInclude(usuario => usuario.RefreshTokens);
    }
}
