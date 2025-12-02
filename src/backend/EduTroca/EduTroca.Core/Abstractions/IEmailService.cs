using EduTroca.Core.Entities.UsuarioAggregate;

namespace EduTroca.Core.Abstractions;
public interface IEmailService
{
    Task SendConfirmationAsync(Usuario usuario);
}
