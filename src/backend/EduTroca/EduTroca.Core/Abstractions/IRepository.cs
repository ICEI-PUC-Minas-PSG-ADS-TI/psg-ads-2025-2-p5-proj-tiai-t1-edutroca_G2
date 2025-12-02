using EduTroca.Core.Common;
using EduTroca.Core.Specifications;

namespace EduTroca.Core.Abstractions;
public interface IRepository<T> where T : class
{
    Task<T> AddAsync(T entity);
    Task RemoveAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<T?> FirstOrDefaultAsync(Specification<T> specification);
    Task<List<T>> ListAsync(Specification<T> specification);
    Task<PagedResult<T>> ListPagedAsync(Specification<T> specification, int pageNumber, int pageSize);
    Task<bool> AnyAsync(Specification<T> specification);
    Task SaveChangesAsync();
}
