using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Enums;
using EduTroca.Core.Specifications;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Usuarios.Certify;
public class CertifyUsuarioCommandHandler(IRepository<Usuario> usuarioRepository)
    : IRequestHandler<CertifyUsuarioCommand, ErrorOr<Success>>
{
    private readonly IRepository<Usuario> _usuarioRepository = usuarioRepository;
    public async Task<ErrorOr<Success>> Handle(CertifyUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuarioByIdSpecification = new UsuarioById(request.usuarioId);
        var usuario = await _usuarioRepository.FirstOrDefaultAsync(usuarioByIdSpecification, cancellationToken);
        if (usuario is null)
            return Error.NotFound("Usuario.NotFound", "Usuario inexistente ou inativo.");
        if (usuario.Nivel is not ENivel.CriadorCertificado)
            usuario.SetNivel(ENivel.CriadorCertificado);
        await _usuarioRepository.UpdateAsync(usuario, cancellationToken);
        await _usuarioRepository.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
