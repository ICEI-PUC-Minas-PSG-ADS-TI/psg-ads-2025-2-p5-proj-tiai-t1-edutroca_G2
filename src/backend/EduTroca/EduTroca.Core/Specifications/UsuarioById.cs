using EduTroca.Core.Entities.UsuarioAggregate;

namespace EduTroca.Core.Specifications;
public class UsuarioById : Specification<Usuario>
{
    public UsuarioById(Guid usuarioId, bool includeDetails = false, bool includeCredentials = false)
        : base(usuario => usuario.Id == usuarioId)
    {
        if (includeDetails)
        {
            AddInclude(usuario => usuario.Roles);
            AddInclude(usuario => usuario.CategoriasDeInteresse);
            AddInclude(usuario => usuario.Likes);
            AddInclude(usuario => usuario.Dislikes);
        }
        if (includeCredentials)
        {
            AddInclude(usuario => usuario.EmailConfirmationCode);
            AddInclude(usuario => usuario.RefreshTokens);
        }
    }
}
