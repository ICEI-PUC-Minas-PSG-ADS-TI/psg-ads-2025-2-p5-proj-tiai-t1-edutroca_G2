namespace EduTroca.Core.Entities;
public class Role
{
    public int Id { get; private set; }
    public string Nome { get; private set; }
    protected Role() { }

    public Role(int id,string name)
    {
        Id = id;
        Nome = name;
    }
}
