using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities;
using EduTroca.Core.Specifications;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Categorias.Delete;
public class DeleteCategoriaCommandHandler(IRepository<Categoria> categoriaRepository) : IRequestHandler<DeleteCategoriaCommand, ErrorOr<Success>>
{
    private readonly IRepository<Categoria> _categoriaRepository = categoriaRepository;
    public async Task<ErrorOr<Success>> Handle(DeleteCategoriaCommand request, CancellationToken cancellationToken)
    {
        var categoriaByIdSpecification = new CategoriaById(request.id);
        var categoria = await _categoriaRepository.FirstOrDefaultAsync(categoriaByIdSpecification);
        if (categoria is null)
            return Error.NotFound("Categoria.NotFound", "Categoria inexistente.");
        await _categoriaRepository.RemoveAsync(categoria);
        await _categoriaRepository.SaveChangesAsync();
        return Result.Success;
    }
}
