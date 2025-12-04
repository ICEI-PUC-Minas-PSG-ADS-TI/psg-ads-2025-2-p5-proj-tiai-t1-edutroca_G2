using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Enums;
using EduTroca.Core.Specifications;
using EduTroca.UseCases.Usuarios.DTOs;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Usuarios.Create;
public class CreateUsuarioCommandHandler(
    IRepository<Usuario> usuarioepository, 
    IRepository<Role> roleRepository, 
    IPasswordHasher passwordHasher, 
    IEmailService emailService) : 
    IRequestHandler<CreateUsuarioCommand, ErrorOr<UsuarioDTO>>
{
    private readonly IRepository<Usuario> _usuarioRepository = usuarioepository;
    private readonly IRepository<Role> _roleRepository = roleRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IEmailService _emailService = emailService;
    public async Task<ErrorOr<UsuarioDTO>> Handle(CreateUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuarioByEmailSpecification = new UsuarioByEmail(request.email);
        var existsByEmail = await _usuarioRepository.AnyAsync(usuarioByEmailSpecification);
        if (existsByEmail)
            return Error.Conflict("Usuario.Email" ,"Email já cadastrado no banco de dados.");

        List<Role> roles = new();

        if (request.rolesIds is not null && request.rolesIds.Count > 0)
        {
            var spec = new RolesByIdsList(request.rolesIds.Cast<int>().ToList());
            var rolesEncontradas = await _roleRepository.ListAsync(spec);

            if (rolesEncontradas.Count != request.rolesIds.Count)
                return Error.Validation("Roles.Invalid", "Uma ou mais roles não existem.");

            roles = rolesEncontradas;
        }
        else
        {
            var defaultRole = await _roleRepository.FirstOrDefaultAsync(new RoleById((int)ERole.User));
            if (defaultRole is null) return Error.Unexpected();
            roles.Add(defaultRole);
        }
        var senhaHash = _passwordHasher.HashPassword(request.senha);
        var newUsuario = await _usuarioRepository.AddAsync(
            new Usuario(
                request.nome,
                request.email,
                senhaHash,
                DateTime.UtcNow.AddMinutes(20),
                roles
                ));
        await _emailService.SendConfirmationAsync(newUsuario);
        await _usuarioRepository.SaveChangesAsync();
        return UsuarioDTO.FromUsuario(newUsuario);
    }
}
