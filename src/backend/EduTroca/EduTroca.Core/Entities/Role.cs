using EduTroca.Core.Enums;

namespace EduTroca.Core.Entities;
public class Role : Entity
{
    public ERole Code { get; set; }
    public string Nome { get; private set; }
    protected Role() { }

    public Role(ERole code,string nome)
    {
        Code = code;
        Nome = nome;
    }
}
