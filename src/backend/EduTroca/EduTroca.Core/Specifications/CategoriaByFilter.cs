using EduTroca.Core.Entities;
using System.Linq.Expressions;

namespace EduTroca.Core.Specifications;
public class CategoriaByFilter : Specification<Categoria>
{
    public CategoriaByFilter(string? nome)
        : base(BuildCriteria(nome))
    {
    }
    private static Expression<Func<Categoria, bool>>? BuildCriteria(string? nome)
    {
        if (string.IsNullOrEmpty(nome))
            return null;

        return c => c.Nome.Contains(nome);
    }
}
