using System.Linq.Expressions;
using TravelConnect.Domain.Entities.Base;

namespace TravelConnect.Domain.Ports.Persistence;

public interface IRepository<T> where T : class, IEntityBase<Guid>
{
    /// <summary>
    /// Obtiene una entidad por su identificador, incluyendo relaciones opcionales.
    /// </summary>
    /// <param name="id">Identificador de la entidad.</param>
    /// <param name="includes">Relaciones opcionales a incluir.</param>
    /// <returns>Entidad encontrada o null.</returns>
    Task<T?> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includes);

    /// <summary>
    /// Obtiene todas las entidades, incluyendo relaciones opcionales.
    /// </summary>
    /// <param name="includes">Relaciones opcionales a incluir.</param>
    /// <returns>Lista de todas las entidades.</returns>
    Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);

    /// <summary>
    /// Busca entidades que cumplan con una condición, incluyendo relaciones opcionales.
    /// </summary>
    /// <param name="predicate">Condición de filtro.</param>
    /// <param name="includes">Relaciones opcionales a incluir.</param>
    /// <returns>Lista de entidades encontradas.</returns>
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

    /// <summary>
    /// Agrega una nueva entidad.
    /// </summary>
    /// <param name="entity">Entidad a agregar.</param>
    Task AddAsync(T entity);

    /// <summary>
    /// Actualiza una entidad existente.
    /// </summary>
    /// <param name="entity">Entidad a actualizar.</param>
    Task UpdateAsync(T entity);

    /// <summary>
    /// Elimina una entidad existente.
    /// </summary>
    /// <param name="entity">Entidad a eliminar.</param>
    Task DeleteAsync(T entity);
}
