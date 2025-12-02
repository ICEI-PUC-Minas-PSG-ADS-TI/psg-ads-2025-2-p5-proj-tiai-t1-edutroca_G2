using EduTroca.Core.Entities;

namespace EduTroca.Core.Specifications;
public class RoleById : Specification<Role>
{
    public RoleById(int id)
        :base(role=>role.Id == id)
    {
    }
}
