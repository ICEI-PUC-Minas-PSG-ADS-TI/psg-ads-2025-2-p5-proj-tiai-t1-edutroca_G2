using EduTroca.Core.Common;
using EduTroca.Core.Enums;
using EduTroca.UseCases.Common.DTOs;
using EduTroca.UseCases.Conteudos.DTOs;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Conteudos.Filter;
public record FilterConteudoQuery(
    string? Titulo,
    RangeFilter<int>? Visualizacoes,
    RangeFilter<int>? Likes,
    DateRangeFilter? Periodo,
    ENivel? Reputacao,
    Guid? AutorId,
    List<Guid>? CategoriasIds,
    EConteudoTipo? tipo,
    EConteudoOrderBy OrderBy,
    PaginationDTO Pagination)
    : IRequest<ErrorOr<PagedResult<ConteudoDTO>>>;