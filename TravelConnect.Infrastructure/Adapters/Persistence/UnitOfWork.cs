using TravelConnect.Domain.Ports.Persistence;
using TravelConnect.Infrastructure.Adapters.Persistence;

namespace TravelConnect.Infrastructure.Persistence;

public class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork, IDisposable
{
    private readonly ApplicationDbContext dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public async Task<int> CommitAsync()
    {
        return await dbContext.SaveChangesAsync();
    }

    public void Rollback()
    {
        this.Rollback();
    }

    public void Dispose()
    {
        dbContext.Dispose();
    }
}