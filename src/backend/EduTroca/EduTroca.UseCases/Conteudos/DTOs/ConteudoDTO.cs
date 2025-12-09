using EduTroca.Core.Entities.ConteudoAggregate;
using EduTroca.Core.Enums;
using EduTroca.UseCases.Categorias.DTOs;
using EduTroca.UseCases.Usuarios.DTOs;

namespace EduTroca.UseCases.Conteudos.DTOs;
public record ConteudoDTO
{
    public Guid Id { get; init; }
    public string Titulo { get; init; } = string.Empty;
    public string Descricao { get; init; } = string.Empty;
    public DateTime DataPublicacao { get; init; }
    public int Visualizacoes { get; init; }
    public int Likes { get; init; }
    public bool Liked { get; init; }
    public int Dislikes { get; init; }
    public bool Disliked { get; init; }
    public EConteudoTipo Tipo { get; init; }
    public string? CaminhoImagem { get; init; }
    public string? CaminhoVideo { get; init; }
    public string? TextoCompleto { get; init; }
    public UsuarioDTO Autor { get; init; } = null!;
    public CategoriaDTO Categoria { get; init; } = null!;
    public List<ComentarioDTO> Comentarios { get; init; } = new();
    public int TotalComentarios { get; init; }

    public static ConteudoDTO FromConteudo(Conteudo conteudo, bool liked = false, bool disliked = false)
    {
        return new ConteudoDTO
        {
            Id = conteudo.Id,
            Titulo = conteudo.Titulo,
            Descricao = conteudo.Descricao,
            DataPublicacao = conteudo.DataPublicacao,
            Visualizacoes = conteudo.Visualizacoes,
            Likes = conteudo.Likes.Count,
            Liked = liked,
            Dislikes = conteudo.Dislikes.Count,
            Disliked = disliked,
            Autor = UsuarioDTO.FromUsuario(conteudo.Autor!),
            Categoria = CategoriaDTO.FromCategoria(conteudo.Categoria!),
            TotalComentarios = conteudo.Comentarios.Count,
            Comentarios = conteudo.Comentarios.Select(c => ComentarioDTO.FromComentario(c)).ToList(),
            Tipo = conteudo is Video ? EConteudoTipo.Video : EConteudoTipo.Pergunta,
            CaminhoVideo = (conteudo as Video)?.CaminhoVideo,
            CaminhoImagem = (conteudo as Video)?.CaminhoImagem,
            TextoCompleto = (conteudo as Pergunta)?.TextoCompleto
        };
    }
}
