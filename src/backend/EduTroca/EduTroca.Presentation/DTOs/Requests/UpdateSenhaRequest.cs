namespace EduTroca.Presentation.DTOs.Requests;
public record UpdateSenhaRequest(string usuarioEmail, string senhaAtual, string senhaNova);
