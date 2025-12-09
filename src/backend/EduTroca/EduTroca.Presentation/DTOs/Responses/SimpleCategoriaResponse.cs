using EduTroca.UseCases.Categorias.DTOs;

namespace EduTroca.Presentation.DTOs.Responses;
public class SimpleCategoriaResponse
{
    public Guid Id { get; }
    public string Nome { get; }

    private SimpleCategoriaResponse(Guid id, string nome)
    {
        Id = id;
        Nome = nome;
    }
    public static SimpleCategoriaResponse FromCategoriaDTO(CategoriaDTO categoria)
    {
        return new SimpleCategoriaResponse(categoria.Id, categoria.Nome);
    }
}
