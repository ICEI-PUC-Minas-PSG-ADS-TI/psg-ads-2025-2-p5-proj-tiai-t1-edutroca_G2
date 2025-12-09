using EduTroca.Core.Entities.ConteudoAggregate;

namespace EduTroca.Core.Specifications;
public class ConteudoByDislike : Specification<Conteudo>
{
    public ConteudoByDislike(Guid usuarioId, bool includeDetails = false)
        : base(c => c.Dislikes.Any(u => u.Id == usuarioId))
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
