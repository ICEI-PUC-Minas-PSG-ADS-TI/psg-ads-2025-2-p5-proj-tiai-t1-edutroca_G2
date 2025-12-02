using EduTroca.Core.Entities;

namespace EduTroca.Core.Specifications;
public class CategoriaByNome : Specification<Categoria>
{
    public CategoriaByNome(string nome)
        : base(c => c.Nome == nome)
    {
    }
    public CategoriaByNome(string nome, Guid ignoreId)
        : base(c => c.Nome == nome && c.Id != ignoreId)
    {
    }
}
