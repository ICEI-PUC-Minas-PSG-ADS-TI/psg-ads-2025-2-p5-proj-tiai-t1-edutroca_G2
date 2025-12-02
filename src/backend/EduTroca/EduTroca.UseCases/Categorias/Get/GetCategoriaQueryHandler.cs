using EduTroca.Core.Abstractions;
using EduTroca.Core.Entities;
using EduTroca.Core.Specifications;
using EduTroca.UseCases.Categorias.DTOs;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Categorias.Get;
public class GetCategoriaQueryHandler(IRepository<Categoria> categoriaRepository) : IRequestHandler<GetCategoriaQuery, ErrorOr<CategoriaDTO>>
{
    private readonly IRepository<Categoria> _categoriaRepository = categoriaRepository;
    public async Task<ErrorOr<CategoriaDTO>> Handle(GetCategoriaQuery request, CancellationToken cancellationToken)
    {
        var categoriaByIdSpecification = new CategoriaById(request.id);
        var categoria = await _categoriaRepository.FirstOrDefaultAsync(categoriaByIdSpecification);
        if (categoria is null)
            return Error.NotFound("Categoria.NotFound", "Categoria inexistente.");
        return CategoriaDTO.FromCategoria(categoria);
    }
}
