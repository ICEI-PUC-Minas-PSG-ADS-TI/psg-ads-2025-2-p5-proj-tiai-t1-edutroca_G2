using EduTroca.Core.Entities.UsuarioAggregate;
using System.Linq.Expressions;

namespace EduTroca.Core.Specifications;
public class UsuarioByFilter : Specification<Usuario>
{
    public UsuarioByFilter(string? nome, List<Guid>? categoriasIds)
        : base(BuildCriteria(nome, categoriasIds))
    {
    }
    private static Expression<Func<Usuario, bool>>? BuildCriteria(string? nome, List<Guid>? categoriasIds)
    {
        var hasNome = !string.IsNullOrEmpty(nome);
        var hasCategorias = categoriasIds is not null && categoriasIds.Count > 0;

        if (!hasNome && !hasCategorias)
            return null;

        return u =>
            (!hasNome || u.Nome.Contains(nome!)) &&
            (!hasCategorias || u.CategoriasDeInteresse.Any(c => categoriasIds!.Contains(c.Id)));
    }
}
