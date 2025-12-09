using EduTroca.UseCases.Common.DTOs;

namespace EduTroca.Presentation.DTOs.Responses;
public class LoginResponse
{
    public CompleteUsuarioResponse Usuario { get; }
    public string AccessToken { get; }
    public string RefreshToken { get; }

    private LoginResponse(CompleteUsuarioResponse usuario, string token, string refreshToken)
    {
        Usuario = usuario;
        AccessToken = token;
        RefreshToken = refreshToken;
    }

    public static LoginResponse FromLoginDTO(LoginDTO loginDTO)
    {
        var usuarioResponse = CompleteUsuarioResponse.FromUsuarioDTO(loginDTO.Usuario);
        return new LoginResponse(usuarioResponse, loginDTO.AccessToken, loginDTO.RefreshToken);
    }
}
