using EduTroca.Core.Common;
using EduTroca.Core.Specifications;

namespace EduTroca.Core.Abstractions;
public interface IRepository<T> where T : class
{
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task RemoveAsync(T entity, CancellationToken cancellationToken = default);
    Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task<T?> FirstOrDefaultAsync(Specification<T>? specification = null, CancellationToken cancellationToken = default);
    Task<List<T>> ListAsync(Specification<T>? specification = null, CancellationToken cancellationToken = default);
    Task<PagedResult<T>> ListPagedAsync(int pageNumber, int pageSize, Specification<T>? specification = null, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Specification<T>? specification = null, CancellationToken cancellationToken = default);
    Task<int> CountAsync(Specification<T>? specification = null, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
