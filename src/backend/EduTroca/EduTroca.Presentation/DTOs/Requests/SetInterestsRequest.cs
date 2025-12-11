namespace EduTroca.Presentation.DTOs.Requests;
public record SetInterestsRequest(Guid usuarioId, List<Guid> categoriasIds);
