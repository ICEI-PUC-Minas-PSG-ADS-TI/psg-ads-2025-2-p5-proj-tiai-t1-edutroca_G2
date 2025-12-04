using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities;
using EduTroca.Core.Entities.UsuarioAggregate;
using EduTroca.Core.Specifications;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Usuarios.AddInterest;
public class AddInterestCommandHandler(
    IRepository<Usuario> usuarioRepository,
    IRepository<Categoria> categoriaRepository) : IRequestHandler<AddInterestCommand, ErrorOr<Success>>
{
    private readonly IRepository<Usuario> _usuarioRepository = usuarioRepository;
    private readonly IRepository<Categoria> _categoriaRepository = categoriaRepository;
    public async Task<ErrorOr<Success>> Handle(AddInterestCommand request, CancellationToken cancellationToken)
    {
        var usuarioByIdSpecification = new UsuarioById(request.usuarioId);
        var usuario = await _usuarioRepository.FirstOrDefaultAsync(usuarioByIdSpecification);
        if (usuario is null)
            return Error.NotFound("Usuario.NotFound", "Usuario inexistente ou inativo.");
        var categoriaByIdSpecification = new CategoriaById(request.categoriaId);
        var categoria = await _categoriaRepository.FirstOrDefaultAsync(categoriaByIdSpecification);
        if(categoria is null)
            return Error.NotFound("Categoria.NotFound", "Categoria inexistente.");
        if (usuario.CategoriasDeInteresse.Contains(categoria))
            return Error.Conflict("Usuario.Categoria","Usuario ja possui interesse nessa categoria.");
        usuario.AddCategoriaDeInteresse(categoria);
        await _usuarioRepository.UpdateAsync(usuario);
        await _usuarioRepository.SaveChangesAsync();
        return Result.Success;
    }
}
