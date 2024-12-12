namespace TravelConnect.Domain.Ports.Persistence;
public interface IUnitOfWork
{
    Task CommitAsync();
    Task RollbackAsync();
}
