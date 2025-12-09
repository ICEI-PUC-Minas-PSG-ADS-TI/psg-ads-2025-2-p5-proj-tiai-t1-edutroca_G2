using EduTroca.Core.Entities.ConteudoAggregate;

namespace EduTroca.Core.Specifications;
public class ConteudoById : Specification<Conteudo>
{
    public ConteudoById(Guid id, bool includeDetails = false)
        : base(c => c.Id == id)
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
