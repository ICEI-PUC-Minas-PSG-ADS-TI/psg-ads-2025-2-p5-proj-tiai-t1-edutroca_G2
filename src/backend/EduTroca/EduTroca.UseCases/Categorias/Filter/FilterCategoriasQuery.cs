using EduTroca.Core.Common;
using EduTroca.UseCases.Categorias.DTOs;
using EduTroca.UseCases.Common.DTOs;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Categorias.Filter;
public record FilterCategoriasQuery(string? nome, PaginationDTO pagination) : IRequest<ErrorOr<PagedResult<CategoriaDTO>>>;
