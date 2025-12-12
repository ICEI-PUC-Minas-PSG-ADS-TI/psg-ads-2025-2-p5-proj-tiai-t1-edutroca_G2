using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities;
using EduTroca.Core.Specifications;
using EduTroca.UseCases.Categorias.DTOs;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Categorias.Create;
public class CreateCategoriaCommandHandler(IRepository<Categoria> categoriaRepository) 
    : IRequestHandler<CreateCategoriaCommand, ErrorOr<Guid>>
{
    private readonly IRepository<Categoria> _categoriaRepository = categoriaRepository;
    public async Task<ErrorOr<Guid>> Handle(CreateCategoriaCommand request, CancellationToken cancellationToken)
    {
        var categoriaByNomeSpecification = new CategoriaByNome(request.nome);
        var categoriaExists = await _categoriaRepository.AnyAsync(categoriaByNomeSpecification);
        if (categoriaExists)
            return Error.Conflict("Categoria.Nome", "Uma categoria com esse nome ja existe.");
        var categoria = new Categoria(request.nome, request.descricao);
        await _categoriaRepository.AddAsync(categoria);
        await _categoriaRepository.SaveChangesAsync();
        return categoria.Id;
    }
}
