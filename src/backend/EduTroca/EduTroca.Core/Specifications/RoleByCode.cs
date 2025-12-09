using EduTroca.Core.Entities;
using EduTroca.Core.Enums;

namespace EduTroca.Core.Specifications;
public class RoleByCode : Specification<Role>
{
    public RoleByCode(ERole code)
        :base(role=>role.Code == code)
    {
    }
}
