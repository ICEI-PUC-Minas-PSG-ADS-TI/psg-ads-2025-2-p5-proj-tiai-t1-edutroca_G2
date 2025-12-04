using EduTroca.Core.Abstractions;
using EduTroca.Core.Common;
using EduTroca.Core.Entities;
using EduTroca.Core.Specifications;
using EduTroca.UseCases.Categorias.DTOs;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Categorias.Filter;
public class FilterCategoriasQueryHandler(IRepository<Categoria> categoriaRepository) : IRequestHandler<FilterCategoriasQuery, ErrorOr<PagedResult<CategoriaDTO>>>
{
    private readonly IRepository<Categoria> _categoriaRepository = categoriaRepository;
    public async Task<ErrorOr<PagedResult<CategoriaDTO>>> Handle(FilterCategoriasQuery request, CancellationToken cancellationToken)
    {
        var categoriaByNomeSpecification = new CategoriaByFilter(request.nome);
        var categorias = await _categoriaRepository.ListPagedAsync
            (categoriaByNomeSpecification,
            request.pagination.PageNumber, 
            request.pagination.PageSize);
        return categorias.Map(c=>CategoriaDTO.FromCategoria(c));
    }
}
