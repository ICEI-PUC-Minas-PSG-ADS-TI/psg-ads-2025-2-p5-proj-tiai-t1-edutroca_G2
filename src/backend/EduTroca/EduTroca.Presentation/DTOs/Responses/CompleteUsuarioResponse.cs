using EduTroca.Core.Enums;
using EduTroca.UseCases.Usuarios.DTOs;

namespace EduTroca.Presentation.DTOs.Responses;
public class CompleteUsuarioResponse
{
    public Guid Id { get; }
    public string Nome { get; }
    public string Email { get; }
    public string Bio { get; }
    public string CaminhoImagem { get; }
    public ENivel Nivel { get; }
    public List<ERole> Roles { get; }
    public List<SimpleCategoriaResponse> CategoriasDeInteresse { get; } = new();
    private CompleteUsuarioResponse(Guid id, string nome, string email, string bio,
        string caminhoImagem, List<SimpleCategoriaResponse> categorias, List<ERole> roles, ENivel nivel)
    {
        Id = id;
        Nome = nome;
        Email = email;
        Bio = bio;
        CaminhoImagem = caminhoImagem;
        CategoriasDeInteresse = categorias;
        Roles = roles;
        Nivel = nivel;
    }
    public static CompleteUsuarioResponse FromUsuarioDTO(UsuarioDTO usuario)
    {
        var categoriasDTO = usuario.CategoriasDeInteresse.Select(x => SimpleCategoriaResponse.FromCategoriaDTO(x)).ToList();
        return new CompleteUsuarioResponse(
            usuario.Id,
            usuario.Nome,
            usuario.Email,
            usuario.Bio,
            usuario.CaminhoImagem,
            categoriasDTO,
            usuario.Roles,
            usuario.Nivel);
    }
}
