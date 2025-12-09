using Microsoft.AspNetCore.Http;

namespace EduTroca.Presentation.DTOs.Requests;
public record UpdateVideoRequest(string titulo, string descricao, Guid categoriaId, IFormFile imagem);
