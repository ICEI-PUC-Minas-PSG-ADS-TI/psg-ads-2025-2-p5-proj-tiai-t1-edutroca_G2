using EduTroca.Core.Enums;
using EduTroca.UseCases.Conteudos.DTOs;

namespace EduTroca.Presentation.DTOs.Responses;
public class CompleteConteudoResponse
{
    public Guid Id { get; init; }
    public string Titulo { get; init; } = string.Empty;
    public string Descricao { get; init; } = string.Empty;
    public string? CaminhoImagem { get; init; }
    public string? CaminhoVideo { get; init; }
    public string? TextoCompleto { get; init; }
    public DateTime DataPublicacao { get; init; }
    public int Visualizacoes { get; init; }
    public int Likes { get; init; }
    public bool Liked { get; init; }
    public int Disikes { get; init; }
    public bool Disliked { get; init; }
    public EConteudoTipo Tipo { get; init; }
    public SimpleUsuarioResponse Autor { get; init; } = null!;
    public SimpleCategoriaResponse Categoria { get; init; } = null!;
    public List<ComentarioResponse> Comentarios { get; init; } = new();
    public int TotalComentarios { get; init; }

    public static CompleteConteudoResponse FromConteudoDTO(ConteudoDTO conteudo)
    {
        return new CompleteConteudoResponse
        {
            Id = conteudo.Id,
            Titulo = conteudo.Titulo,
            Descricao = conteudo.Descricao,
            DataPublicacao = conteudo.DataPublicacao,
            Visualizacoes = conteudo.Visualizacoes,
            Likes = conteudo.Likes,
            Liked = conteudo.Liked,
            Disikes = conteudo.Dislikes,
            Disliked = conteudo.Disliked,
            Tipo = conteudo.Tipo,
            Autor = SimpleUsuarioResponse.FromUsuarioDTO(conteudo.Autor),
            Categoria = SimpleCategoriaResponse.FromCategoriaDTO(conteudo.Categoria),
            Comentarios = conteudo.Comentarios.Select(x => ComentarioResponse.FromComentarioDTO(x)).ToList(),
            TotalComentarios = conteudo.TotalComentarios,
            CaminhoVideo = conteudo.CaminhoVideo,
            CaminhoImagem = conteudo.CaminhoImagem,
            TextoCompleto = conteudo.TextoCompleto
        };
    }
}