using EduTroca.Core.Entities;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Enums;

namespace EduTroca.Tests.Helpers;
public static class UsuarioHelpers
{
    public static Usuario CreateUsuario()
    {
        return new Usuario(
            "Teste",
            "teste@email.com",
            "hash",
            DateTime.UtcNow,
            [new Role((int)ERole.User, "User")]
        );
    }
    public static Usuario CreateUsuario(string nome = "Teste", string email = "teste@email.com", string senhaHash = "hash", string? currentPicturePath = null)
    {
        var usuario = new Usuario(
            nome,
            email,
            senhaHash,
            DateTime.UtcNow,
            [new Role((int)ERole.User, "User")]
        );
        if (currentPicturePath != null)
            usuario.UpdatePicture(currentPicturePath);
        return usuario;
    }
    public static Usuario CreateUsuario(DateTime emailConfirmationExpiresOnUtc)
    {
        return new Usuario(
            "Teste",
            "teste@email.com",
            "hash",
            emailConfirmationExpiresOnUtc,
            [new Role((int)ERole.User, "User")]
        );
    }
    public static Usuario CreateUsuario(bool isConfirmed)
    {
        var usuario = new Usuario(
            "Teste",
            "teste@email.com",
            "hash",
            DateTime.UtcNow,
            [new Role((int)ERole.User, "User")]
        );
        if (isConfirmed)
            usuario.ConfirmEmail();
        return usuario;
    }
    public static Usuario CreateUsuario(ERole role)
    {
        return new Usuario(
            "Teste",
            "teste@email.com",
            "hash",
            DateTime.UtcNow,
            [new Role((int)role, role.ToString())]
        );
    }
}
