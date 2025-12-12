using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Specifications;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Usuarios.SetInterests;
public class SetInterestsCommandHandler(
    IRepository<Usuario> usuarioRepository,
    IRepository<Categoria> categoriaRepository) : IRequestHandler<SetInterestsCommand, ErrorOr<Success>>
{
    private readonly IRepository<Usuario> _usuarioRepository = usuarioRepository;
    private readonly IRepository<Categoria> _categoriaRepository = categoriaRepository;
    public async Task<ErrorOr<Success>> Handle(SetInterestsCommand request, CancellationToken cancellationToken)
    {
        var usuarioByIdSpecification = new UsuarioById(request.usuarioId, includeDetails: true);
        var usuario = await _usuarioRepository.FirstOrDefaultAsync(usuarioByIdSpecification);
        if (usuario is null)
            return Error.NotFound("Usuario.NotFound", "Usuario inexistente ou inativo.");
        var categoriasByIdsListSpec = new CategoriasByIdsList(request.categoriasIds);
        var categorias = await _categoriaRepository.ListAsync(categoriasByIdsListSpec);
        if (categorias.Count != request.categoriasIds.Count)
            return Error.NotFound("Categoria.NotFound", "Uma ou mais categorias são inexistentes.");
        usuario.SetCategoriasDeInteresse(categorias);
        await _usuarioRepository.UpdateAsync(usuario);
        await _usuarioRepository.SaveChangesAsync();
        return Result.Success;
    }
}
