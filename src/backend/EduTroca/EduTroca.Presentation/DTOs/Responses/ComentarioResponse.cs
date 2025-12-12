using EduTroca.UseCases.Conteudos.DTOs;

namespace EduTroca.Presentation.DTOs.Responses;
public class ComentarioResponse
{
    public Guid Id { get; init; }
    public string Texto { get; init; }
    public DateTime DataPublicacao { get; init; }
    public SimpleUsuarioResponse Autor { get; init; }
    public static ComentarioResponse FromComentarioDTO(ComentarioDTO comentario)
    {
        return new ComentarioResponse
        {
            Id = comentario.Id,
            Texto = comentario.Texto,
            DataPublicacao = comentario.DataPublicacao,
            Autor = SimpleUsuarioResponse.FromUsuarioDTO(comentario.Autor)
        };
    }
}
