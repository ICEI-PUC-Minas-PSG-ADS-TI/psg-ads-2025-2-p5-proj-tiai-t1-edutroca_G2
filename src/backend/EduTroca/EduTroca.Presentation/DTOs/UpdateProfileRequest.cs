using Microsoft.AspNetCore.Http;

namespace EduTroca.Presentation.DTOs;
public record UpdateProfileRequest(string nome, string bio, IFormFile? profilePicture, bool removePicture);
