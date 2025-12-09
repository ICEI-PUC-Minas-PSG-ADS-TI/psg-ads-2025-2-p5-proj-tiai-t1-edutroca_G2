using EduTroca.Core.Abstractions;
using EduTroca.Core.Common;
using EduTroca.Core.Entities;
using EduTroca.Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace EduTroca.Infraestructure.Persistence.Repositories;
public class EfRepository<T>(AppDbContext context) : IRepository<T> where T : Entity
{
    private readonly AppDbContext _context = context;

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
    {
        await _context.Set<T>().AddAsync(entity,cancellationToken);
        return entity;
    }
    public Task RemoveAsync(T entity, CancellationToken cancellationToken)
    {
        _context.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }
    public Task<T> UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        _context.Set<T>().Update(entity);
        return Task.FromResult(entity);
    }
    public async Task<T?> FirstOrDefaultAsync(Specification<T>? specification, CancellationToken cancellationToken)
    {
        var query = ApplySpecification(specification);
        return await query.FirstOrDefaultAsync(cancellationToken);
    }
    public async Task<List<T>> ListAsync(Specification<T>? specification, CancellationToken cancellationToken)
    {
        var query = ApplySpecification(specification);
        return await query.ToListAsync(cancellationToken);
    }
    public async Task<PagedResult<T>> ListPagedAsync( int pageNumber, int pageSize, Specification<T>? specification, CancellationToken cancellationToken)
    {
        var queryForCount = _context.Set<T>().AsQueryable();
        if (specification?.Criteria is not null)
        {
            queryForCount = queryForCount.Where(specification.Criteria);
        }
        var totalCount = await queryForCount.CountAsync(cancellationToken);

        var query = ApplySpecification(specification);

        if (specification?.OrderByExpression is null && specification?.OrderByDescendingExpression is null)
            query = query.OrderBy(x => x.Id);

        var skip = (pageNumber - 1) * pageSize;
        var pagedQuery = query.Skip(skip).Take(pageSize);

        var items = await pagedQuery.ToListAsync(cancellationToken);

        return new PagedResult<T>(items, totalCount, pageNumber, pageSize);
    }
    public async Task<bool> AnyAsync(Specification<T>? specification, CancellationToken cancellationToken)
    {
        var query = ApplySpecification(specification);
        return await query.AnyAsync(cancellationToken);
    }
    public async Task<int> CountAsync(Specification<T>? specification, CancellationToken cancellationToken)
    {
        var query = ApplySpecification(specification);
        return await query.CountAsync(cancellationToken);
    }
    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        UpdateSoftDeleteEntities();
        return _context.SaveChangesAsync(cancellationToken);
    }
    private void UpdateSoftDeleteEntities()
    {
        var entries = _context.ChangeTracker.Entries<ISoftDelete>();
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Entity.Delete();
                entry.Property(x => x.IsDeleted).IsModified = true;
                entry.Property(x => x.DeletedOnUtc).IsModified = true;
            }
        }
    }
    private IQueryable<T> ApplySpecification(Specification<T>? spec)
    {
        return SpecificationEvaluator.GetQuery(_context.Set<T>().AsQueryable(), spec);
    }
}
