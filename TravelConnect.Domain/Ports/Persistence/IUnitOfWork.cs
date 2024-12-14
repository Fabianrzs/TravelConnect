namespace TravelConnect.Domain.Ports.Persistence;
public interface IUnitOfWork
{
    Task<int> CommitAsync();
    void Rollback();
}
