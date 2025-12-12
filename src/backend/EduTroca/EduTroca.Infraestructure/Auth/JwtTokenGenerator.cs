using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities.UsuarioAggregate;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EduTroca.Infraestructure.Auth;
public class JwtTokenGenerator(IOptions<JwtSettings> jwtOptions) : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;

    public string GenerateAccessToken(Usuario usuario)
    {
        var handler = new JwtSecurityTokenHandler();

        var claims = GenerateClaims(usuario);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            SigningCredentials = creds,
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience
        };

        var token = handler.CreateToken(descriptor);

        return handler.WriteToken(token);
    }

    private static ClaimsIdentity GenerateClaims(Usuario usuario)
    {
        var claims = new ClaimsIdentity();
        claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()));
        claims.AddClaim(new Claim(ClaimTypes.Name, usuario.Nome));
        claims.AddClaim(new Claim(ClaimTypes.Email, usuario.Email));
        foreach (var role in usuario.Roles)
            claims.AddClaim(new Claim(ClaimTypes.Role, role.Nome));
        return new ClaimsIdentity(claims);
    }

    public RefreshToken GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        var token = Convert.ToBase64String(randomBytes);
        return new RefreshToken(token, DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays));
    }
}
