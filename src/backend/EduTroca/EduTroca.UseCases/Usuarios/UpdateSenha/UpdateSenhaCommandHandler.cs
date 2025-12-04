using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Enums;
using EduTroca.Core.Specifications;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Usuarios.UpdateSenha;
public class UpdateSenhaCommandHandler(
    IPasswordHasher passwordHasher,
    IRepository<Usuario> usuarioRepository,
    ICurrentUserService currentUser)
    : IRequestHandler<UpdateSenhaCommand, ErrorOr<Success>>
{
    private readonly IRepository<Usuario> _usuarioRepository = usuarioRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly ICurrentUserService _currentUser = currentUser;

    public async Task<ErrorOr<Success>> Handle(UpdateSenhaCommand request, CancellationToken cancellationToken)
    {
        var usuarioByIdSpecification = new UsuarioById(request.usuarioId);
        var usuario = await _usuarioRepository.FirstOrDefaultAsync(usuarioByIdSpecification);

        if (usuario is null)
            return Error.NotFound("Usuario.NotFound", "Usuario inexistente ou inativo.");

        if (!_currentUser.IsInRole(ERole.Admin))

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
