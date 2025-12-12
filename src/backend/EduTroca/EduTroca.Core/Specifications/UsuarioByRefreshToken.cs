using EduTroca.Core.Entities.UsuarioAggregate;

namespace EduTroca.Core.Specifications;
public class UsuarioByRefreshToken : Specification<Usuario>
{
    public UsuarioByRefreshToken(string refreshToken)
        : base(usuario => usuario.RefreshTokens.Any(x => x.Token == refreshToken))
    {
        AddInclude(usuario => usuario.RefreshTokens);
    }
}
