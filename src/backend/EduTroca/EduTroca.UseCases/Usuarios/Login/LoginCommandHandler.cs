using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Specifications;
using EduTroca.UseCases.Usuarios.DTOs;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Usuarios.Login;
public class LoginCommandHandler
    (IRepository<Usuario> usuarioRepository, 
    IPasswordHasher passwordHasher, 
    IJwtTokenGenerator jwtTokenGenerator) : IRequestHandler<LoginCommand, ErrorOr<LoginDTO>>
{
    private readonly IRepository<Usuario> _usuarioRepository = usuarioRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;
    public async Task<ErrorOr<LoginDTO>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var usuarioByEmailSpecification = new UsuarioByEmail(request.email);
        var usuario = await _usuarioRepository.FirstOrDefaultAsync(usuarioByEmailSpecification);
        if (usuario is null)
            return Error.NotFound("Usuario.NotFound", "Email e/ou senha incorreta.");
        if (!_passwordHasher.VerifyPassword(request.senha, usuario.SenhaHash))
            return Error.NotFound("Usuario.NotFound", "Email e/ou senha incorreta.");
        if (!usuario.IsConfirmed)
            return Error.Forbidden("Usuario.NotConfirmedEmail", "Email não confirmado");
        var token = _jwtTokenGenerator.GenerateAccessToken(usuario);
        var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();
        usuario.AddRefreshToken(refreshToken);
        await _usuarioRepository.UpdateAsync(usuario);
        await _usuarioRepository.SaveChangesAsync();
        return LoginDTO.FromUsuario(usuario, token, refreshToken.Token);
    }
}
