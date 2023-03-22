using System.Linq.Expressions;
using System.Reflection;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.DataAccess;

public class EfBaseRepository<TEntity, TContext> : IRepository<TEntity>
    where TEntity : BaseEntity
    where TContext : DbContext
{
    private TContext Context { get; }

    public EfBaseRepository(TContext context)
    {
        Context = context;
    }

    public DbSet<TEntity> Table => Context.Set<TEntity>();
    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await Table.AddAsync(entity);
        return entity;
    }

    public async Task<List<TEntity>> AddRangeAsync(List<TEntity> entities)
    {
        await Table.AddRangeAsync(entities);
        return entities;
    }

    public TEntity Update(TEntity entity)
    {
        Table.Update(entity);
        return entity;
    }
    public List<TEntity> UpdateRange(List<TEntity> entities)
    {
        Table.UpdateRange(entities);
        return entities;
    }
    public async Task<TEntity?> RemoveAsync(int id)
    {
        var entity = await Table.FirstOrDefaultAsync(p => p.Id == id);
        if (entity != null)
        {
            Remove(entity);
            return entity;
        }

        return null;
    }

    public TEntity Remove(TEntity entity)
    {
        Table.Remove(entity);
        return entity;
    }

    public List<TEntity> RemoveRange(List<TEntity> entities)
    {
        Table.RemoveRange(entities);
        return entities;
    }

    public async Task<int> SaveAsync()
        => await Context.SaveChangesAsync();
    public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null, bool tracking = true)
    {
        var query = Table.AsQueryable();
        if (!tracking)
            query = query.AsNoTracking();
        return filter == null ? query : query.Where(filter);
    }

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter, bool tracking = true)
    {
        var query = Table.AsQueryable();
        if (!tracking)
            query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync(filter);
    }

    public async Task<TEntity?> GetByIdAsync(int id, bool tracking = true)
        => await GetAsync(entity => entity.Id == id, tracking);

    public (List<TEntity> entities, int totalCount) GetWithPagination(BasePaginationRequest req, Expression<Func<TEntity, bool>>? filter = null)
    {
        var query = GetAll().AsQueryable();
        query = CheckIfRequestParams(query, req, filter);

        return (query.Skip(req.Index * req.Size).Take(req.Size).ToList()
                , query.Count());
    }

    protected IQueryable<TEntity> CheckIfRequestParams(IQueryable<TEntity> query, BasePaginationRequest req, Expression<Func<TEntity, bool>>? filter = null)
    {
        if (!string.IsNullOrEmpty(req.OrderByField) && !string.IsNullOrEmpty(req.OrderType))
        {
            var propertyInfo = typeof(TEntity).GetProperty(req.OrderByField, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo != null)
            
                query = req.OrderType.ToLower() == "desc" 
                    ? query.AsEnumerable().OrderByDescending(x => propertyInfo.GetValue(x, null)).AsQueryable()
                    : query.AsEnumerable().OrderBy(x => propertyInfo.GetValue(x, null)).AsQueryable();
        }
        else
            query = query.OrderByDescending(x => x.Id);
        

        if (!string.IsNullOrEmpty(req.SearchValue) && !string.IsNullOrEmpty(req.SearchValueField))
        {
            var propertyInfo = typeof(TEntity).GetProperty(req.SearchValueField, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo != null)
                query = query.AsEnumerable().Where(x => propertyInfo.GetValue(x, null).ToString().Contains(req.SearchValue)).AsQueryable();
            
        }

        return filter == null
            ? query
            : query.Where(filter);
    }
}