using EduTroca.Core.Entities.ConteudoAggregate;
using EduTroca.Core.Entities.UsuarioAggregate;

namespace EduTroca.Core.Entities;
public class Comentario : Entity
{
    public string Texto { get; private set; }
    public DateTime DataPublicacao { get; private set; }
    public Guid AutorId { get; private set; }
    public Usuario? Autor { get; private set; }
    public Guid ConteudoId { get; private set; }
    public Conteudo? Conteudo { get; private set; }
    protected Comentario() { }

    public Comentario(string texto, Guid autorId, Guid conteudoId)
    {
        Texto = texto;
        AutorId = autorId;
        ConteudoId = conteudoId;
        DataPublicacao = DateTime.UtcNow;
    }
}
