namespace EduTroca.Core.Entities;
public class Categoria
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public string Descricao { get; private set; }
    protected Categoria()
    {
    }

    public Categoria(string nome, string descricao)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Descricao = descricao;
    }

    public void Update(string nome, string descricao)
    {
        Nome = nome;
        Descricao = descricao;
    }
}
