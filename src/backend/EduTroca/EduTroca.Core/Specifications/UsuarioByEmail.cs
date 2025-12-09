using EduTroca.Core.Entities.UsuarioAggregate;

namespace EduTroca.Core.Specifications;
public class UsuarioByEmail : Specification<Usuario>
{
    public UsuarioByEmail(string email, bool includeDetails = false, bool includeCredentials = false)
    : base(usuario => usuario.Email == email)
    {
        if (includeDetails)
        {
            AddInclude(usuario => usuario.Roles);
            AddInclude(usuario => usuario.CategoriasDeInteresse);
        }
        if (includeCredentials)
        {
            AddInclude(usuario => usuario.EmailConfirmationCode);
            AddInclude(usuario => usuario.RefreshTokens);
        }
    }
}
