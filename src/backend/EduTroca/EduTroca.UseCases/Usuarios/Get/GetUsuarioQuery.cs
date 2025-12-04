using EduTroca.UseCases.Usuarios.DTOs;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Usuarios.Get;
public record GetUsuarioQuery(Guid usuarioId) : IRequest<ErrorOr<UsuarioDTO>>;
