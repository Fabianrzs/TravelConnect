using System.Linq.Expressions;
using TravelConnect.Domain.Entities.Base;

namespace TravelConnect.Domain.Ports;
public interface IRepository<T> where T : class, IEntityBase<Guid>
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
