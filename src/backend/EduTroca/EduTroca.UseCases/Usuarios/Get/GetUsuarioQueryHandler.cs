using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Specifications;
using EduTroca.UseCases.Usuarios.DTOs;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Usuarios.Get;
public class GetUsuarioQueryHandler(IRepository<Usuario> usuarioRepository) : IRequestHandler<GetUsuarioQuery, ErrorOr<UsuarioDTO>>
{
    private readonly IRepository<Usuario> _usuarioRepository = usuarioRepository;
    public async Task<ErrorOr<UsuarioDTO>> Handle(GetUsuarioQuery request, CancellationToken cancellationToken)
    {
        var usuarioByIdSpecification = new UsuarioById(request.usuarioId);
        var usuario = await _usuarioRepository.FirstOrDefaultAsync(usuarioByIdSpecification);
        if (usuario is null)
            return Error.NotFound("Usuario.NotFound", "Usuario inexistente ou inativo.");
        return UsuarioDTO.FromUsuario(usuario);
    }
}
