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

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await dbSet.ToListAsync();
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

    public Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return Task.FromResult<IEnumerable<T>>([.. dbSet.Where(predicate)]);
    }
}
