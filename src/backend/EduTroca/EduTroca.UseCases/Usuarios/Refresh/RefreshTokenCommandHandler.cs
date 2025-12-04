using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Specifications;
using EduTroca.UseCases.Usuarios.DTOs;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Usuarios.Refresh;
public class RefreshTokenCommandHandler(
    IRepository<Usuario> usuarioRepository,
    IJwtTokenGenerator jwtTokenGenerator)
    : IRequestHandler<RefreshTokenCommand, ErrorOr<LoginDTO>>
{
    private readonly IRepository<Usuario> _usuarioRepository = usuarioRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;
    public async Task<ErrorOr<LoginDTO>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var usuarioByRefreshTokenSpecification = new UsuarioByRefreshToken(request.refreshToken);
        var usuario = await _usuarioRepository.FirstOrDefaultAsync(usuarioByRefreshTokenSpecification);

        if (usuario is null)
            return Error.Unauthorized("RefreshToken.Invalid", "Refresh Token inválido.");
        var existingToken = usuario.FindRefreshToken(request.refreshToken);
        if (existingToken is null)
            return Error.Unauthorized("Token.Invalid", "Token não encontrado.");
        if (existingToken.IsRevoked)
            return Error.Unauthorized("Token.Revoked", "Tentativa de reuso de token detectada. Faça login novamente.");
        if (existingToken.IsExpired)
            return Error.Unauthorized("Token.Expired", "Token expirado. Faça login novamente.");
        var newAccessToken = _jwtTokenGenerator.GenerateAccessToken(usuario);
        var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken();
        existingToken.Revoke();
        usuario.AddRefreshToken(newRefreshToken);
        usuario.RemoveOldRefreshTokens(30);
        await _usuarioRepository.UpdateAsync(usuario);
        await _usuarioRepository.SaveChangesAsync();
        return LoginDTO.FromUsuario(usuario, newAccessToken, newRefreshToken.Token);
    }
}
