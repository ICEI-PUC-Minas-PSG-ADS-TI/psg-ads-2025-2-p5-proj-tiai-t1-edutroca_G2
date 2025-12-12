using EduTroca.Core.Entities.ConteudoAggregate;

namespace EduTroca.Core.Specifications;
public class ConteudoByCategoria : Specification<Conteudo>
{
    public ConteudoByCategoria(Guid categoriaId, bool includeDetails = false)
        : base(c => c.CategoriaId == categoriaId)
    {
        if (includeDetails)
        {
            AddInclude(c => c.Autor);
            AddInclude(c => c.Categoria);
            AddInclude(c => c.Comentarios);
            AddInclude(c => c.Likes);
            AddInclude(c => c.Dislikes);
        }
    }
}
