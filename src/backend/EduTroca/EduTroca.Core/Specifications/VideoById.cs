using EduTroca.Core.Entities.ConteudoAggregate;

namespace EduTroca.Core.Specifications;
public class VideoById : Specification<Video>
{
    public VideoById(Guid id, bool includeDetails = false)
        : base(v => v.Id == id)
    {
        if (includeDetails)
        {
            AddInclude(v => v.Autor);
            AddInclude(v => v.Categoria);
        }
    }
}
