using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Enums;

namespace EduTroca.Core.Entities.ConteudoAggregate;
public abstract class Conteudo : Entity
{
    public string Titulo { get; protected set; }
    public string Descricao { get; protected set; }
    public DateTime DataPublicacao { get; protected set; }
    public int Visualizacoes { get; protected set; }
    public EConteudoTipo Tipo { get; protected set; }
    public Guid AutorId { get; protected set; }
    public Usuario? Autor { get; protected set; }
    public Guid CategoriaId { get; protected set; }
    public Categoria? Categoria { get; protected set; }
    protected readonly List<Usuario> _likes = new();
    public IReadOnlyCollection<Usuario> Likes => _likes.AsReadOnly();
    protected readonly List<Usuario> _dislikes = new();
    public IReadOnlyCollection<Usuario> Dislikes => _dislikes.AsReadOnly();
    protected readonly List<Comentario> _comentarios = new();
    public IReadOnlyCollection<Comentario> Comentarios => _comentarios.AsReadOnly();

    protected Conteudo() { }

    protected Conteudo(string titulo, string descricao, Guid autorId, Guid categoriaId)
        : base()
    {
        Titulo = titulo;
        Descricao = descricao;
        AutorId = autorId;
        CategoriaId = categoriaId;
        DataPublicacao = DateTime.UtcNow;
        Visualizacoes = 0;
    }
    public void AdicionarLike(Usuario usuario)
    {
        _likes.Add(usuario);
    }
    public void RemoverLike(Usuario usuario)
    {
        _likes.Remove(usuario);
    }
    public void AdicionarDislike(Usuario usuario)
    {
        _dislikes.Add(usuario);
    }
    public void RemoverDislike(Usuario usuario)
    {
        _dislikes.Remove(usuario);
    }
    public void RegistrarVisualizacao()
    {
        Visualizacoes++;
    }
}
