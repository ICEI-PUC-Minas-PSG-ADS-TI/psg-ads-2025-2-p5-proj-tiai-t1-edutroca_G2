namespace EduTroca.Presentation.DTOs.Requests;
public record UpdatePerguntaRequest(string titulo, string descricao, Guid categoriaId, string texto);
