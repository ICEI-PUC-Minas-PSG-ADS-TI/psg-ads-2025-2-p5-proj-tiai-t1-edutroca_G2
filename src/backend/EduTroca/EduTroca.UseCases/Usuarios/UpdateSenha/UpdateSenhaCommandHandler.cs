using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Specifications;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Usuarios.UpdateSenha;
public class UpdateSenhaCommandHandler(
    IPasswordHasher passwordHasher,
    IRepository<Usuario> usuarioRepository)
    : IRequestHandler<UpdateSenhaCommand, ErrorOr<Success>>
{
    private readonly IRepository<Usuario> _usuarioRepository = usuarioRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async Task<ErrorOr<Success>> Handle(UpdateSenhaCommand request, CancellationToken cancellationToken)
    {
        var usuarioByIdSpecification = new UsuarioByEmail(request.usuarioEmail, includeCredentials: true);
        var usuario = await _usuarioRepository.FirstOrDefaultAsync(usuarioByIdSpecification);

        if (usuario is null)
            return Error.NotFound("Usuario.NotFound", "Usuario inexistente ou inativo.");

        if (!_passwordHasher.VerifyPassword(request.senhaAtual!, usuario.SenhaHash))
            return Error.Validation("Senha.Incorreta", "A senha atual está incorreta.");

        var senhaNovaHash = _passwordHasher.HashPassword(request.senhaNova);

        usuario.UpdateSenha(senhaNovaHash);
        usuario.RevokeAllRefreshTokens();

        await _usuarioRepository.UpdateAsync(usuario);
        await _usuarioRepository.SaveChangesAsync();

        return Result.Success;
    }
}
