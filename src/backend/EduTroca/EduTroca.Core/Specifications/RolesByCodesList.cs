using EduTroca.Core.Entities;
using EduTroca.Core.Enums;

namespace EduTroca.Core.Specifications;
public class RolesByCodesList : Specification<Role>
{
    public RolesByCodesList(List<ERole> codesList)
        : base(role => codesList.Contains(role.Code))
    {
    }
}