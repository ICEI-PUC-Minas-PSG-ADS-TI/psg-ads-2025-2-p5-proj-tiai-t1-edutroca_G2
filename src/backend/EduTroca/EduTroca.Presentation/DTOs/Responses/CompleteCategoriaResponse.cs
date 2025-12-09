using EduTroca.UseCases.Categorias.DTOs;

namespace EduTroca.Presentation.DTOs.Responses;
public class CompleteCategoriaResponse
{
    public Guid Id { get; }
    public string Nome { get; }
    public string Descricao { get; }

    private CompleteCategoriaResponse(Guid id, string nome, string descricao)
    {
        Id = id;
        Nome = nome;
        Descricao = descricao;
    }
    public static CompleteCategoriaResponse FromCategoriaDTO(CategoriaDTO categoria)
    {
        return new CompleteCategoriaResponse(categoria.Id, categoria.Nome, categoria.Descricao);
    }
}
