using EduTroca.Core.Entities.UsuarioAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduTroca.Core.Specifications;
public class UsuarioByRefreshToken : Specification<Usuario>
{
    public UsuarioByRefreshToken(string refreshToken)
        : base(usuario => usuario.RefreshTokens.Any(x => x.Token == refreshToken))
    {
        AddInclude(usuario => usuario.CategoriasDeInteresse);
        AddInclude(usuario => usuario.EmailConfirmationCode);
        AddInclude(usuario => usuario.RefreshTokens);
    }
}
