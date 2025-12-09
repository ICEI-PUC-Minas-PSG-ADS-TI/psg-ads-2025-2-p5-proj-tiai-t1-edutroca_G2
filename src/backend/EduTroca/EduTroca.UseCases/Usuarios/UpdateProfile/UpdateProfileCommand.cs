using EduTroca.UseCases.Common.DTOs;
using EduTroca.UseCases.Usuarios.DTOs;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Usuarios.UpdateProfile;
public record UpdateProfileCommand(
    Guid usuarioId,
    string nome,
    string bio,
    FileDTO? picture,
    bool DeletePicture
) : IRequest<ErrorOr<UsuarioDTO>>;