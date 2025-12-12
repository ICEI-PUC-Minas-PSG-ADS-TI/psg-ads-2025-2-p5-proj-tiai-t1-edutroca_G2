using EduTroca.UseCases.Common.DTOs;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Usuarios.Login;
public record LoginCommand(string email, string senha) : IRequest<ErrorOr<LoginDTO>>;
