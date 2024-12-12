using Microsoft.EntityFrameworkCore;
using TravelConnect.Domain.Entities;
using TravelConnect.Domain.Entities.Base;

namespace TravelConnect.Infrastructure.Adapters.Persistence.DbContexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<EmergencyContact> EmergenciesContacts { get; set; }
    public DbSet<Guest> Guests { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<TravelAgent> TravelsAgents { get; set; }

    public async Task CommitAsync()
    {
        await SaveChangesAsync().ConfigureAwait(false);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(DomainEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.Name).Property<DateTime>("CreatedOn").HasDefaultValueSql("GETDATE()");
                modelBuilder.Entity(entityType.Name).Property<DateTime>("LastModifiedOn").HasDefaultValueSql("GETDATE()");
            }
        }

        base.OnModelCreating(modelBuilder);
    }
}
