using EduTroca.Core.Entities.ConteudoAggregate;

namespace EduTroca.Core.Specifications;
public class ConteudoByAutor : Specification<Conteudo>
{
    public ConteudoByAutor(Guid autorId, bool includeDetails = false)
        : base(c => c.AutorId == autorId)
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
