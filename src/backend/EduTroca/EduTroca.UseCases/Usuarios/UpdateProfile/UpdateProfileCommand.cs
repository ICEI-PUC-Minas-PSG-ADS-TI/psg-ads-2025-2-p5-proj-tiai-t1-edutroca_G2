using EduTroca.UseCases.Common.DTOs;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Usuarios.UpdateProfile;
public record UpdateProfileCommand(
    Guid usuarioId,
    string nome,
    string bio,
    PictureDTO? picture,
    bool DeletePicture
) : IRequest<ErrorOr<Success>>;