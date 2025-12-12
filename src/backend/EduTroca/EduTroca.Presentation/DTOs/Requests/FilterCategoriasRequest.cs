namespace EduTroca.Presentation.DTOs.Requests;
public record FilterCategoriasRequest(string? nome, int pageNumber = 1, int pageSize = 20);
