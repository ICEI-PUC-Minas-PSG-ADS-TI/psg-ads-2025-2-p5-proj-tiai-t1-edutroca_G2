using EduTroca.Core.Entities.UsuarioAggregate;

namespace EduTroca.Core.Specifications;
public class UsuarioById : Specification<Usuario>
{
    public UsuarioById(Guid usuarioId)
        : base(usuario => usuario.Id == usuarioId)
    {
        AddInclude(usuario => usuario.CategoriasDeInteresse);
        AddInclude(usuario => usuario.EmailConfirmationCode);
        AddInclude(usuario => usuario.RefreshTokens);
    }
}
