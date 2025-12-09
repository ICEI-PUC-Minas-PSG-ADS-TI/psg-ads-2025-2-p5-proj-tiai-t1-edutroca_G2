using EduTroca.UseCases.Usuarios.DTOs;

namespace EduTroca.UseCases.Common.DTOs;
public class LoginDTO
{
    public UsuarioDTO Usuario { get; }
    public string AccessToken { get; }
    public string RefreshToken { get; }

    public LoginDTO(UsuarioDTO usuario, string token, string refreshToken)
    {
        Usuario = usuario;
        AccessToken = token;
        RefreshToken = refreshToken;
    }
}
