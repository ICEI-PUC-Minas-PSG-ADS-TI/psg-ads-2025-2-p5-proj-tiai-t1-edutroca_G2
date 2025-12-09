using EduTroca.UseCases.Usuarios.DTOs;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Usuarios.UpdateEmail;
public record UpdateEmailCommand(Guid usuarioId,string novoEmail) : IRequest<ErrorOr<UsuarioDTO>>;
