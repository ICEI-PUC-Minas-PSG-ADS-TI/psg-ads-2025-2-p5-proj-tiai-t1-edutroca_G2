using EduTroca.Core.Entities;

namespace EduTroca.Core.Specifications;
public class RolesByIdsList : Specification<Role>
{
    public RolesByIdsList(List<int> idsList)
        : base(role => idsList.Contains(role.Id))
    {
    }
}