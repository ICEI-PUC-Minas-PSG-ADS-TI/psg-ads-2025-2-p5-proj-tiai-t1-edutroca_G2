using EduTroca.Core.Enums;
using EduTroca.UseCases.Usuarios.DTOs;

namespace EduTroca.Presentation.DTOs.Responses;
public class SimpleUsuarioResponse
{
    public Guid Id { get; }
    public string Nome { get; }
    public string CaminhoImagem { get; }
    public ENivel Nivel { get; }
    private SimpleUsuarioResponse(Guid id, string nome,
        string caminhoImagem, ENivel nivel)
    {
        Id = id;
        Nome = nome;
        CaminhoImagem = caminhoImagem;
        Nivel = nivel;
    }
    public static SimpleUsuarioResponse FromUsuarioDTO(UsuarioDTO usuario)
    {
        return new SimpleUsuarioResponse(
            usuario.Id,
            usuario.Nome,
            usuario.CaminhoImagem,
            usuario.Nivel);
    }
}
