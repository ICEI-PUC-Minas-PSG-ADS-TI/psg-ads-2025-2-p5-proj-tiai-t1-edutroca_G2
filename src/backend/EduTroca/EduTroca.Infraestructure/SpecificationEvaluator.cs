using EduTroca.Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace EduTroca.Infraestructure;
public static class SpecificationEvaluator
{
    public static IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, Specification<T>? specification)
        where T : class
    {
        if (specification == null)
            return inputQuery;

        var query = inputQuery;
        if (specification.Criteria is not null)
            query = query.Where(specification.Criteria);

        query = specification.IncludeExpressions.Aggregate(query,
                (current, include) => current.Include(include));

        if (specification.OrderByExpression is not null)
            query = query.OrderBy(specification.OrderByExpression);
        else if (specification.OrderByDescendingExpression is not null)
            query = query.OrderByDescending(specification.OrderByDescendingExpression);

        return query;
    }
}
