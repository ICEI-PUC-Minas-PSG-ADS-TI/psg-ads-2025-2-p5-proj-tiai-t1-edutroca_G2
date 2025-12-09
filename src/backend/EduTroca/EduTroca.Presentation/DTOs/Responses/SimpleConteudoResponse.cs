using EduTroca.Core.Enums;
using EduTroca.UseCases.Conteudos.DTOs;

namespace EduTroca.Presentation.DTOs.Responses;
public class SimpleConteudoResponse
{
    public Guid Id { get; init; }
    public string Titulo { get; init; } = string.Empty;
    public string Descricao { get; init; } = string.Empty;
    public string? CaminhoImagem { get; init; } = string.Empty;
    public DateTime DataPublicacao { get; init; }
    public int Visualizacoes { get; init; }
    public int Likes { get; init; }
    public EConteudoTipo Tipo { get; init; }
    public SimpleUsuarioResponse Autor { get; init; } = null!;
    public SimpleCategoriaResponse Categoria { get; init; } = null!;
    public int TotalComentarios { get; init; }

    public static SimpleConteudoResponse FromConteudoDTO(ConteudoDTO conteudo)
    {
        return new SimpleConteudoResponse
        {
            Id = conteudo.Id,
            Titulo = conteudo.Titulo,
            Descricao = conteudo.Descricao,
            DataPublicacao = conteudo.DataPublicacao,
            Visualizacoes = conteudo.Visualizacoes,
            Likes = conteudo.Likes,
            Tipo = conteudo.Tipo,
            Autor = SimpleUsuarioResponse.FromUsuarioDTO(conteudo.Autor),
            Categoria = SimpleCategoriaResponse.FromCategoriaDTO(conteudo.Categoria),
            TotalComentarios = conteudo.TotalComentarios,
            CaminhoImagem = conteudo.CaminhoImagem
        };
    }
}
