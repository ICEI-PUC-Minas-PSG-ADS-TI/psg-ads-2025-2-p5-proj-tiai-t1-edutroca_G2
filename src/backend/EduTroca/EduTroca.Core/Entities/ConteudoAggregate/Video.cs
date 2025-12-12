namespace EduTroca.Core.Entities.ConteudoAggregate;
public class Video : Conteudo
{
    public string CaminhoVideo { get; private set; }
    public string CaminhoImagem { get; private set; }

    private Video() { }

    public Video(string titulo, string descricao, Guid autorId, 
        Guid categoriaId, string caminhoVideo, string caminhoImagem)
        : base(titulo, descricao, autorId, categoriaId)
    {
        CaminhoVideo = caminhoVideo;
        CaminhoImagem = caminhoImagem;
    }
    public void Update(string titulo, string descricao, Guid categoriaId, string caminhoImagem)
    {
        Titulo = titulo;
        Descricao = descricao;
        CategoriaId = categoriaId;
        CaminhoImagem = caminhoImagem;
    }
}
