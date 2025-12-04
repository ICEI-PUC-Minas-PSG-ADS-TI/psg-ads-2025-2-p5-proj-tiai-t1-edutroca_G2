namespace EduTroca.Presentation.DTOs;
public record UpdateSenhaRequest(Guid usuarioId, string? senhaAtual, string senhaNova);
