using EduTroca.Core.Entities.UsuarioAggregate;

namespace EduTroca.UseCases.Usuarios.DTOs;
public class LoginDTO
{
    public UsuarioDTO Usuario { get; }
    public string AccessToken { get; }
    public string RefreshToken { get; }

    private LoginDTO(UsuarioDTO usuario, string token, string refreshToken)
    {
        Usuario = usuario;
        AccessToken = token;
        RefreshToken = refreshToken;
    }
    public static LoginDTO FromUsuario(Usuario usuario, string token, string refreshToken)
    {
        return new LoginDTO(UsuarioDTO.FromUsuario(usuario), token, refreshToken);
    }
}
