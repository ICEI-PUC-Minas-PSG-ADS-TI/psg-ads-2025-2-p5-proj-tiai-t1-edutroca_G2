using EduTroca.Core.Abstractions;
using EduTroca.Core.Common;
using EduTroca.Core.Entities.ConteudoAggregate;
using EduTroca.Core.Specifications;
using EduTroca.UseCases.Conteudos.DTOs;
using ErrorOr;
using MediatR;

namespace EduTroca.UseCases.Conteudos.Filter;
public class FilterConteudoQueryHandler(IRepository<Conteudo> conteudoRepository)
    : IRequestHandler<FilterConteudoQuery, ErrorOr<PagedResult<ConteudoDTO>>>
{
    private readonly IRepository<Conteudo> _conteudoRepository = conteudoRepository;

    public async Task<ErrorOr<PagedResult<ConteudoDTO>>> Handle(
        FilterConteudoQuery request,
        CancellationToken cancellationToken)
    {
        var conteudosSpec = new ConteudoByFilterSpec(
            request.Titulo,
            request.Visualizacoes,
            request.Likes,
            request.Periodo,
            request.Reputacao,
            request.AutorId,
            request.CategoriasIds,
            request.tipo,
            request.OrderBy,
            true);

        var conteudos = await _conteudoRepository.ListPagedAsync(
            request.Pagination.PageNumber,
            request.Pagination.PageSize,
            conteudosSpec,
            cancellationToken);

        return conteudos.Map(x=>ConteudoDTO.FromConteudo(x));
    }
}
