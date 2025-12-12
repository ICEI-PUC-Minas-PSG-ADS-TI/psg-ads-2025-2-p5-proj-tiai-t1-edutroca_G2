namespace EduTroca.Core.Entities.ConteudoAggregate;
public class Pergunta : Conteudo
{
    public string TextoCompleto { get; private set; }

    private Pergunta() { }

    public Pergunta(string titulo, string descricao, Guid autorId, Guid categoriaId, string textoCompleto)
        : base(titulo, descricao, autorId, categoriaId)
    {
        TextoCompleto = textoCompleto;
    }
    public void Update(string titulo, string descricao, Guid categoriaId, string newTexto)
    {
        Titulo = titulo;
        Descricao = descricao;
        CategoriaId = categoriaId;
        TextoCompleto = newTexto;
    }
}
