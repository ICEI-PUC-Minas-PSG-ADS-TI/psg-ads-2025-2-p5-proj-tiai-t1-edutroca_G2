using EduTroca.Core.Entities.UsuarioAggregate;

namespace EduTroca.Core.Specifications;
public class UsuarioByEmail : Specification<Usuario>
{
    public UsuarioByEmail(string email)
    : base(usuario => usuario.Email == email)
    {
        AddInclude(usuario => usuario.CategoriasDeInteresse);
        AddInclude(usuario => usuario.EmailConfirmationCode);
        AddInclude(usuario => usuario.RefreshTokens);
    }
}
