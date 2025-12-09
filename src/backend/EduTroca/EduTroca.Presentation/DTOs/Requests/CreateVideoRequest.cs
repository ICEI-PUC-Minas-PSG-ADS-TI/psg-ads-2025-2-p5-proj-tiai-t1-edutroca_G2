using Microsoft.AspNetCore.Http;

namespace EduTroca.Presentation.DTOs.Requests;
public record CreateVideoRequest(string titulo, string descricao, Guid categoriaId, IFormFile video, IFormFile imagem);
