using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Specifications;
using EduTroca.UseCases.Usuarios.DTOs;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Usuarios.UpdateRoles;
public class UpdateRolesCommandHandler(
    IRepository<Usuario> usuarioRepository,
    IRepository<Role> roleRepository)
    : IRequestHandler<UpdateRolesCommand, ErrorOr<UsuarioDTO>>
{
    private readonly IRepository<Usuario> _usuarioRepository = usuarioRepository;
    private readonly IRepository<Role> _roleRepository = roleRepository;
    public async Task<ErrorOr<UsuarioDTO>> Handle(UpdateRolesCommand request, CancellationToken cancellationToken)
    {
        var usuarioByIdSpec = new UsuarioById(request.usuarioId, includeDetails: true);
        var usuario = await _usuarioRepository.FirstOrDefaultAsync(usuarioByIdSpec);
        if (usuario is null)
            return Error.NotFound("Usuario.NotFound", "Usuario inexistente ou inativo.");

        var rolesByIdsListSpec = new RolesByCodesList(request.rolesCodes);
        var roles = await _roleRepository.ListAsync(rolesByIdsListSpec);
        if (roles.Count != request.rolesCodes.Count)
            return Error.Validation("Roles.Invalid", "Uma ou mais roles não existem.");

        usuario.SetRoles(roles);
        await _usuarioRepository.UpdateAsync(usuario);
        await _usuarioRepository.SaveChangesAsync();
        return UsuarioDTO.FromUsuario(usuario);
    }
}
