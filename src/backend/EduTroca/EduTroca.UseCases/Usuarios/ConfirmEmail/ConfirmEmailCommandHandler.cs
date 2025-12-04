using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Specifications;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Usuarios.ConfirmEmail;
public class ConfirmEmailCommandHandler(IRepository<Usuario> usuarioRepository) : IRequestHandler<ConfirmEmailCommand, ErrorOr<Success>>
{
    private readonly IRepository<Usuario> _usuarioRepository = usuarioRepository;
    public async Task<ErrorOr<Success>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var usuarioByEmailConfirmationCode = new UsuarioByEmailConfirmationId(request.id);
        var usuario = await _usuarioRepository.FirstOrDefaultAsync(usuarioByEmailConfirmationCode);
        if(usuario is null)
            return Error.NotFound("Usuario.NotFound", "Usuario inexistente ou inativo.");
        if (usuario.IsConfirmed)
            return Error.Conflict("Usuario.IsConfirmed", "Email de usuario ja confirmado.");
        if (usuario.EmailConfirmationCode.ExpiresOnUtc < DateTime.UtcNow)
            return Error.Conflict("Usuario.Expired", "Email de confirmação expirado.");
        usuario.ConfirmEmail();
        await _usuarioRepository.UpdateAsync(usuario);
        await _usuarioRepository.SaveChangesAsync();
        return Result.Success;
    }
}
