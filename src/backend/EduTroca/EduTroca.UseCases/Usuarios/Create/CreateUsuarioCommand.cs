using EduTroca.Core.Enums;
using EduTroca.UseCases.Usuarios.DTOs;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Usuarios.Create;
public record CreateUsuarioCommand(string nome, string email, string senha, List<ERole>? rolesCodes = null) 
    : IRequest<ErrorOr<UsuarioDTO>>;
