using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Enums;
using EduTroca.UseCases.Categorias.DTOs;

namespace EduTroca.UseCases.Usuarios.DTOs;
public class UsuarioDTO
{
    public Guid Id { get; }
    public string Nome { get; }
    public string Email { get; }
    public string Bio { get; }
    public string CaminhoImagem { get; }
    public List<string> Roles { get; }
    public List<CategoriaDTO> CategoriasDeInteresse { get; } = new();
    private UsuarioDTO(Guid id, string nome, string email, string bio, string caminhoImagem, List<CategoriaDTO> categorias, List<string> roles)
    {
        Id = id;
        Nome = nome;
        Email = email;
        Bio = bio;
        CaminhoImagem = caminhoImagem;
        CategoriasDeInteresse = categorias;
        Roles = roles;
    }
    public static UsuarioDTO FromUsuario(Usuario usuario)
    {
        var categoriasDTO = usuario.CategoriasDeInteresse.Select(x=>CategoriaDTO.FromCategoria(x)).ToList();
        var rolesNames = usuario.Roles.Select(x=>x.Nome).ToList();
        return new UsuarioDTO(usuario.Id, usuario.Nome, usuario.Email, usuario.Bio, usuario.CaminhoImagem, categoriasDTO, rolesNames);
    }
}
