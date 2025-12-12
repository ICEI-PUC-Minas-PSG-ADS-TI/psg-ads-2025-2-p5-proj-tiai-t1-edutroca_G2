using Microsoft.AspNetCore.Http;

namespace EduTroca.Presentation.DTOs.Requests;
public record UpdateProfileRequest(string nome, string bio, IFormFile? profilePicture, bool removePicture);
