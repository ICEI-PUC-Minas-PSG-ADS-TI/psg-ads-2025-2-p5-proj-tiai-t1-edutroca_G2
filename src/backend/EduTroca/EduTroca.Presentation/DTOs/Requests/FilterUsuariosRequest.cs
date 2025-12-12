namespace EduTroca.Presentation.DTOs.Requests;
public record FilterUsuariosRequest(string? nome, List<Guid>? categoriasIds, int pageNumber = 1, int pageSize = 20);
