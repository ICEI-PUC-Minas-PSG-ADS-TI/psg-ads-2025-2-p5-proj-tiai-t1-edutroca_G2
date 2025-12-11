using EduTroca.Core.Entities.ConteudoAggregate;

namespace EduTroca.Core.Specifications;
public class ConteudoByLike : Specification<Conteudo>
{
    public ConteudoByLike(Guid conteudoId, Guid usuarioId, bool includeDetails = false)
        : base(c => c.Id == conteudoId && c.Likes.Any(u => u.Id == usuarioId))
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
