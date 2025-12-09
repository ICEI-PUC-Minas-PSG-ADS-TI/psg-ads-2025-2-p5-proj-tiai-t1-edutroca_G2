using EduTroca.Core.Common;
using EduTroca.Core.Entities.ConteudoAggregate;
using EduTroca.Core.Enums;
using System.Linq.Expressions;

namespace EduTroca.Core.Specifications;
public class ConteudoByFilterSpec : Specification<Conteudo>
{
    public ConteudoByFilterSpec(
        string? titulo,
        RangeFilter<int>? visualizacoes,
        RangeFilter<int>? likes,
        DateRangeFilter? periodo,
        ENivel? reputacao,
        Guid? autorId,
        List<Guid>? categoriasIds,
        EConteudoTipo? tipo,
        EConteudoOrderBy orderBy,
        bool includeDetails = false)
        : base(BuildCriteria(titulo, visualizacoes, likes, periodo, reputacao, autorId, categoriasIds, tipo))
    {
        if (includeDetails)
        {
            AddInclude(c => c.Autor);
            AddInclude(c => c.Categoria);
            AddInclude(c => c.Comentarios);
            AddInclude(c => c.Likes);
            AddInclude(c => c.Dislikes);
        }

        ApplyOrdering(orderBy);
    }

    private static Expression<Func<Conteudo, bool>>? BuildCriteria(
        string? titulo,
        RangeFilter<int>? visualizacoes,
        RangeFilter<int>? likes,
        DateRangeFilter? periodo,
        ENivel? reputacao,
        Guid? autorId,
        List<Guid>? categoriasIds,
        EConteudoTipo? tipo)
    {
        Expression<Func<Conteudo, bool>> criteria = c => true;

        if (!string.IsNullOrWhiteSpace(titulo))
            criteria = criteria.And(c => c.Titulo.Contains(titulo));

        if (visualizacoes?.Min.HasValue == true)
            criteria = criteria.And(c => c.Visualizacoes >= visualizacoes.Min.Value);

        if (visualizacoes?.Max.HasValue == true)
            criteria = criteria.And(c => c.Visualizacoes <= visualizacoes.Max.Value);

        if (likes?.Min.HasValue == true)
            criteria = criteria.And(c => c.Likes.Count >= likes.Min.Value);

        if (likes?.Max.HasValue == true)
            criteria = criteria.And(c => c.Likes.Count <= likes.Max.Value);


        if (periodo?.From.HasValue == true)
            criteria = criteria.And(c => c.DataPublicacao >= periodo.From.Value);
        if (periodo?.To.HasValue == true)
            criteria = criteria.And(c => c.DataPublicacao <= periodo.To.Value);

        if (reputacao.HasValue == true)
            criteria = criteria.And(c => c.Autor!.Nivel == reputacao);

        if (autorId.HasValue)
            criteria = criteria.And(c => c.AutorId == autorId.Value);

        if (categoriasIds is not null && categoriasIds.Any())
            criteria = criteria.And(c => categoriasIds.Contains(c.CategoriaId));

        if (tipo.HasValue)
            criteria = criteria.And(c => c.Tipo == tipo);

        return criteria;
    }

    private void ApplyOrdering(EConteudoOrderBy orderBy)
    {
        switch (orderBy)
        {
            case EConteudoOrderBy.MaisRecente:
                AddOrderByDescending(c => c.DataPublicacao);
                break;
            case EConteudoOrderBy.MaisAntigo:
                AddOrderBy(c => c.DataPublicacao);
                break;
            case EConteudoOrderBy.MaisVisualizacoes:
                AddOrderByDescending(c => c.Visualizacoes);
                break;
            case EConteudoOrderBy.MenosVisualizacoes:
                AddOrderBy(c => c.Visualizacoes);
                break;
            case EConteudoOrderBy.MaisLikes:
                AddOrderByDescending(c => c.Likes);
                break;
            case EConteudoOrderBy.MenosLikes:
                AddOrderBy(c => c.Likes);
                break;
            case EConteudoOrderBy.MaiorReputacao:
                AddOrderByDescending(c => c.Autor!.Nivel);
                break;
            case EConteudoOrderBy.MenorReputacao:
                AddOrderBy(c => c.Autor!.Nivel);
                break;
            default:
                AddOrderByDescending(c => c.DataPublicacao);
                break;
        }
    }
}
