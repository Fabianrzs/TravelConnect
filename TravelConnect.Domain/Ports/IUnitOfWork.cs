namespace TravelConnect.Domain.Ports;
public interface IUnitOfWork
{
    Task CommitAsync();
    Task RollbackAsync();
}
