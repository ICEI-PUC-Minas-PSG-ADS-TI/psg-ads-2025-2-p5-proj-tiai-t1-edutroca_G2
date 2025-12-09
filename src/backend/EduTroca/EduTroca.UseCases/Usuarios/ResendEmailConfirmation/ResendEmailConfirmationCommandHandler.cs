using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Specifications;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Usuarios.ResendEmailConfirmation;
public class ResendEmailConfirmationCommandHandler(IRepository<Usuario> usuarioRepository, IEmailService emailService) : IRequestHandler<ResendEmailConfirmationCommand, ErrorOr<Success>>
{
    private readonly IRepository<Usuario> _usuarioRepository = usuarioRepository;
    private readonly IEmailService _emailService = emailService;
    public async Task<ErrorOr<Success>> Handle(ResendEmailConfirmationCommand request, CancellationToken cancellationToken)
    {
        var usuarioByEmailSpecification = new UsuarioByEmail(request.email, includeCredentials: true);
        var usuario = await _usuarioRepository.FirstOrDefaultAsync(usuarioByEmailSpecification);
        if (usuario is null)
            return Error.NotFound("Usuario.NotFound", "Usuario inexistente ou inativo.");
        if (usuario.IsConfirmed)
            return Error.Conflict("Usuario.IsConfirmed", "Email de usuario ja confirmado.");
        usuario.RegenerateEmailConfirmationCode(DateTime.UtcNow.AddMinutes(20));
        await _usuarioRepository.UpdateAsync(usuario);
        await _emailService.SendConfirmationAsync(usuario);
        await _usuarioRepository.SaveChangesAsync();
        return Result.Success;
    }
}
