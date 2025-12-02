using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities;
using EduTroca.Core.Specifications;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Categorias.Update;
public class UpdateCategoriaCommandHandler(IRepository<Categoria> categoriaRepository) : IRequestHandler<UpdateCategoriaCommand, ErrorOr<Success>>
{
    private readonly IRepository<Categoria> _categoriaRepository = categoriaRepository;
    public async Task<ErrorOr<Success>> Handle(UpdateCategoriaCommand request, CancellationToken cancellationToken)
    {
        var categoriaByIdSpecification = new CategoriaById(request.id);
        var categoria = await _categoriaRepository.FirstOrDefaultAsync(categoriaByIdSpecification);
        if (categoria is null)
            return Error.NotFound("Categoria.NotFound", "Categoria inexistente.");
        var categoriaByNomeSpecification = new CategoriaByNome(request.nome, request.id);
        var categoriaExistsByNome = await _categoriaRepository.AnyAsync(categoriaByNomeSpecification);
        if (categoriaExistsByNome)
            return Error.Conflict("Categoria.Nome", "Uma categoria com esse nome ja existe.");
        categoria.Update(request.nome, request.descricao);
        await _categoriaRepository.UpdateAsync(categoria);
        await _categoriaRepository.SaveChangesAsync();
        return Result.Success;
    }
}
