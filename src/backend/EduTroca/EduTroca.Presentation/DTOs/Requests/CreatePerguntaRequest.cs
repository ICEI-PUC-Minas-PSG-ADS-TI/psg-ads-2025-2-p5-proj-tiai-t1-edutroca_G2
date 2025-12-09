namespace EduTroca.Presentation.DTOs.Requests;
public record CreatePerguntaRequest(string titulo, string descricao, Guid categoriaId, string texto);
