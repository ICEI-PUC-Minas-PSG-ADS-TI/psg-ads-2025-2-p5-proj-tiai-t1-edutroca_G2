using EduTroca.Core.Entities;

namespace EduTroca.Core.Specifications;
public class CategoriaById : Specification<Categoria>
{
    public CategoriaById(Guid id)
        :base(c=>c.Id == id)
    {
    }
}
