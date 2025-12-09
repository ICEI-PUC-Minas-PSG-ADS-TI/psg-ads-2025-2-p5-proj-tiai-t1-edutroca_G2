using EduTroca.Core.Entities;

namespace EduTroca.UseCases.Categorias.DTOs;
public class CategoriaDTO
{
    public Guid Id { get; }
    public string Nome { get; }
    public string Descricao { get; }

    private CategoriaDTO(Guid id, string nome, string descricao)
    {
        Id = id;
        Nome = nome;
        Descricao = descricao;
    }
    public static CategoriaDTO FromCategoria(Categoria categoria)
    {
        return new CategoriaDTO(categoria.Id, categoria.Nome, categoria.Descricao);
    }
}
