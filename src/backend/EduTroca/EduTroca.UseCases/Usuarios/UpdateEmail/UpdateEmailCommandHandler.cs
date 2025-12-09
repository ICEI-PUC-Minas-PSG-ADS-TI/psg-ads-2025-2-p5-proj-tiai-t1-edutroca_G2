using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Specifications;
using EduTroca.UseCases.Usuarios.DTOs;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Usuarios.UpdateEmail;
public class UpdateEmailCommandHandler(IRepository<Usuario> usuarioRepository, IEmailService emailService) 
    : IRequestHandler<UpdateEmailCommand, ErrorOr<UsuarioDTO>>
{
    private readonly IRepository<Usuario> _usuarioRepository = usuarioRepository;
    private readonly IEmailService _emailService = emailService;
    public async Task<ErrorOr<UsuarioDTO>> Handle(UpdateEmailCommand request, CancellationToken cancellationToken)
    {
        var usuarioByIdSpecification = new UsuarioById(request.usuarioId, includeCredentials: true);
        var usuario = await _usuarioRepository.FirstOrDefaultAsync(usuarioByIdSpecification);
        if(usuario is null)
            return Error.NotFound("Usuario.NotFound", "Usuario inexistente ou inativo.");
        if (usuario.Email == request.novoEmail)
            return Error.Conflict("Usuario.Email", "Novo email não pode ser igual ao atual.");
        var usuarioByEmailSpecification = new UsuarioByEmail(request.novoEmail);
        var usuarioEmailExists = await _usuarioRepository.AnyAsync(usuarioByEmailSpecification);
        if (usuarioEmailExists)
            return Error.Conflict("Usuario.Email", "Email já cadastrado no banco de dados.");
        usuario.UpdateEmail(request.novoEmail, DateTime.UtcNow.AddMinutes(20));
        usuario.RevokeAllRefreshTokens();
        await _usuarioRepository.UpdateAsync(usuario);
        await _emailService.SendConfirmationAsync(usuario);
        await _usuarioRepository.SaveChangesAsync();
        return UsuarioDTO.FromUsuario(usuario);
    }
}
