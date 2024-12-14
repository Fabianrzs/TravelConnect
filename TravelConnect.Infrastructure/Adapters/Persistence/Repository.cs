using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TravelConnect.Domain.Entities.Base;
using TravelConnect.Domain.Ports.Persistence;

namespace TravelConnect.Infrastructure.Adapters.Persistence;

public class Repository<T> : IRepository<T> where T : EntityBase
{
    protected readonly ApplicationDbContext _context;
    private readonly DbSet<T> dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        dbSet = _context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = dbSet;

        // Agrega las relaciones incluidas explícitamente
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.FirstOrDefaultAsync(entity => entity.Id == id);
    }

    public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = dbSet;

        // Agrega las relaciones incluidas explícitamente
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await dbSet.AddAsync(entity);
    }

    public Task UpdateAsync(T entity)
    {
        dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity)
    {
        dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = dbSet.Where(predicate);

        // Agrega las relaciones incluidas explícitamente
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.ToListAsync();
    }
}
