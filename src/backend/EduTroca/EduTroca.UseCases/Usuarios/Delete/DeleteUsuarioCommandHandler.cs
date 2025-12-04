using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Specifications;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Usuarios.Delete;
public class DeleteUsuarioCommandHandler(IRepository<Usuario> usuarioRepository) : IRequestHandler<DeleteUsuarioCommand, ErrorOr<Success>>
{
    private readonly IRepository<Usuario> _usuarioRepository = usuarioRepository;
    public async Task<ErrorOr<Success>> Handle(DeleteUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuarioByIdSpecification = new UsuarioById(request.usuarioId);
        var usuario = await _usuarioRepository.FirstOrDefaultAsync(usuarioByIdSpecification);
        if (usuario is null)
            return Error.NotFound("Usuario.NotFound", "Usuario inexistente ou inativo.");
        await _usuarioRepository.RemoveAsync(usuario);
        await _usuarioRepository.SaveChangesAsync();
        return Result.Success;
    }
}
