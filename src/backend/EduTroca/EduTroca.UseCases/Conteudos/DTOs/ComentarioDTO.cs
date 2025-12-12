using EduTroca.Core.Entities;
using EduTroca.UseCases.Usuarios.DTOs;

namespace EduTroca.UseCases.Conteudos.DTOs;
public class ComentarioDTO
{
    public Guid Id { get; init; }
    public string Texto { get; init; }
    public DateTime DataPublicacao { get; init; }
    public UsuarioDTO Autor { get; init; }
    public static ComentarioDTO FromComentario(Comentario comentario)
    {
        return new ComentarioDTO
        {
            Id = comentario.Id,
            Texto = comentario.Texto,
            DataPublicacao = comentario.DataPublicacao,
            Autor = UsuarioDTO.FromUsuario(comentario.Autor!)
        };
    }
}
