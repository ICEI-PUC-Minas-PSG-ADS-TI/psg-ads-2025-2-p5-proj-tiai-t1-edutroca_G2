using EduTroca.Core.Common;
using EduTroca.Core.Enums;

namespace EduTroca.Presentation.DTOs.Requests;
public record FilterConteudosRequest(
    string? Titulo,
    RangeFilter<int>? Visualizacoes,
    RangeFilter<int>? Likes,
    DateRangeFilter? Periodo,
    ENivel? NivelUsuario,
    Guid? AutorId,
    List<Guid>? CategoriasIds,
    EConteudoTipo? Tipo,
    EConteudoOrderBy OrderBy,
    int pageNumber = 1, int pageSize = 20);
