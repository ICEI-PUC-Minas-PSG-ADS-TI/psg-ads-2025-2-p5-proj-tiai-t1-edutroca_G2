using EduTroca.Core.Abstractions;
using EduTroca.Core.Common;
using EduTroca.Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace EduTroca.Infraestructure.Persistence.Repositories;
public class EfRepository<T>(AppDbContext context) : IRepository<T> where T : class
{
    private readonly AppDbContext _context = context;

    public async Task<T> AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        return entity;
    }
    public Task RemoveAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }
    public Task<T> UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        return Task.FromResult(entity);
    }
    public async Task<T?> FirstOrDefaultAsync(Specification<T> specification)
    {
        var query = ApplySpecification(specification);
        return await query.FirstOrDefaultAsync();
    }
    public async Task<List<T>> ListAsync(Specification<T> specification)
    {
        var query = ApplySpecification(specification);
        return await query.ToListAsync();
    }
    public async Task<PagedResult<T>> ListPagedAsync(Specification<T> specification, int pageNumber, int pageSize)
    {
        var queryForCount = _context.Set<T>().AsQueryable();
        if (specification.Criteria is not null)
        {
            queryForCount = queryForCount.Where(specification.Criteria);
        }
        var totalCount = await queryForCount.CountAsync();

        var query = ApplySpecification(specification);

        if (specification.OrderByExpression is null && specification.OrderByDescendingExpression is null)
            query = ApplyDefaultOrderBy(query);

        var skip = (pageNumber - 1) * pageSize;
        var pagedQuery = query.Skip(skip).Take(pageSize);

        var items = await pagedQuery.ToListAsync();

        return new PagedResult<T>(items, totalCount, pageNumber, pageSize);
    }
    public async Task<bool> AnyAsync(Specification<T>? specification)
    {
        var query = specification is not null ? ApplySpecification(specification) : _context.Set<T>().AsQueryable();
        return await query.AnyAsync();
    }
    public Task SaveChangesAsync()
    {
        UpdateSoftDeleteEntities();
        return _context.SaveChangesAsync();
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
    private IQueryable<T> ApplySpecification(Specification<T> spec)
    {
        return SpecificationEvaluator.GetQuery(_context.Set<T>().AsQueryable(), spec);
    }
    private IQueryable<T> ApplyDefaultOrderBy(IQueryable<T> query)
    {
        var entityType = _context.Model.FindEntityType(typeof(T));
        var primaryKeyName = entityType?
            .FindPrimaryKey()?
            .Properties
            .Select(x => x.Name)
            .FirstOrDefault();

        if (!string.IsNullOrEmpty(primaryKeyName))
            return query.OrderBy(x => EF.Property<object>(x, primaryKeyName));
        else
            return query.OrderBy(x => EF.Property<object>(x, "Id"));
    }
}
