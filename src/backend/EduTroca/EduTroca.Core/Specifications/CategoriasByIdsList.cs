using EduTroca.Core.Entities;

namespace EduTroca.Core.Specifications;
public class CategoriasByIdsList : Specification<Categoria>
{
    public CategoriasByIdsList(List<Guid> idsList)
        : base(c => idsList.Contains(c.Id))
    {
    }
}
