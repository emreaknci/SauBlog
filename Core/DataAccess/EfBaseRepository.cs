using System.Linq.Expressions;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

    public (List<TEntity> entities, int totalCount) GetWithPagination(int index, int size, bool tracking = true, Expression<Func<TEntity, bool>>? filter = null)
    {
        return filter == null 
            ? (GetAll().Skip(index * size).Take(size).ToList()
                , GetAll().Count()) 
            : (GetAll().Where(filter).Skip(index * size).Take(size).ToList()
                , GetAll().Where(filter).Count());

    }
}