using EduTroca.UseCases.Usuarios.DTOs;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Usuarios.Refresh;
public record RefreshTokenCommand(string refreshToken) : IRequest<ErrorOr<LoginDTO>>;
