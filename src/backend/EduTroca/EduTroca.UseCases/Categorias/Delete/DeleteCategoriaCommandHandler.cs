using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities;
using EduTroca.Core.Entities.ConteudoAggregate;
using EduTroca.Core.Specifications;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Categorias.Delete;
public class DeleteCategoriaCommandHandler(
    IRepository<Categoria> categoriaRepository,
    IRepository<Conteudo> conteudoRepository)
    : IRequestHandler<DeleteCategoriaCommand, ErrorOr<Success>>
{
    private readonly IRepository<Categoria> _categoriaRepository = categoriaRepository;
    private readonly IRepository<Conteudo> _conteudoRepository = conteudoRepository;
    public async Task<ErrorOr<Success>> Handle(DeleteCategoriaCommand request, CancellationToken cancellationToken)
    {
        var categoriaByIdSpec = new CategoriaById(request.id);
        var categoria = await _categoriaRepository.FirstOrDefaultAsync(categoriaByIdSpec);
        if (categoria is null)
            return Error.NotFound("Categoria.NotFound", "Categoria inexistente.");
        var conteudoByCategoriaSpec = new ConteudoByCategoria(categoria.Id);
        var anyConteudo = await _conteudoRepository.AnyAsync(conteudoByCategoriaSpec);
        if (anyConteudo)
            return Error.Validation("Categoria.Conteudos", "Categoria a ser deletada não pode conter conteudos.");
        await _categoriaRepository.RemoveAsync(categoria);
        await _categoriaRepository.SaveChangesAsync();
        return Result.Success;
    }
}
