using EduTroca.Core.Entities.ConteudoAggregate;
using EduTroca.Core.Entities.UsuarioAggregate;

namespace EduTroca.Core.Entities;
public class Categoria : Entity
{
    public string Nome { get; private set; }
    public string Descricao { get; private set; }
    private readonly List<Conteudo> _conteudos = new();
    public IReadOnlyCollection<Conteudo> Conteudos => _conteudos.AsReadOnly();
    private readonly List<Usuario> _interessados = new();
    public IReadOnlyCollection<Usuario> Interessados => _interessados.AsReadOnly();

    protected Categoria() { }

    public Categoria(string nome, string descricao)
    {
        Nome = nome;
        Descricao = descricao;
    }

    public void Update(string nome, string descricao)
    {
        Nome = nome;
        Descricao = descricao;
    }
}
