using EduTroca.Core.Entities.ConteudoAggregate;

namespace EduTroca.Core.Specifications;
public class PerguntaById : Specification<Pergunta>
{
    public PerguntaById(Guid id, bool includeDetails = false)
        : base(p => p.Id == id)
    {
        if (includeDetails)
        {
            AddInclude(p => p.Autor);
            AddInclude(p => p.Categoria);
        }
    }
}
