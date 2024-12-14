using Microsoft.EntityFrameworkCore;
using TravelConnect.Domain.Entities;
using TravelConnect.Domain.Entities.Base;

namespace TravelConnect.Infrastructure.Adapters.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<EmergencyContact> EmergenciesContacts { get; set; }
    public DbSet<Guest> Guests { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<TravelAgent> TravelsAgents { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added) 
                entry.Property("CreatedOn")
                    .CurrentValue = DateTime.UtcNow;
            if (entry.State == EntityState.Modified) 
                entry.Property("LastModifiedOn")
                    .CurrentValue = DateTime.UtcNow;
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Room>()
            .HasOne(r => r.Hotel)
            .WithMany(h => h.Rooms)
            .HasForeignKey(r => r.HotelId)
            .IsRequired();

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Room)
            .WithMany(rm => rm.Reservations)
            .HasForeignKey(r => r.RoomId)
            .IsRequired();

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.EmergencyContact)
            .WithOne()
            .HasForeignKey<Reservation>(r => r.EmergencyContactId)
            .IsRequired();

        modelBuilder.Entity<Guest>()
            .HasOne(g => g.Reservation)
            .WithMany(r => r.Guests)
            .HasForeignKey(g => g.ReservationId)
            .IsRequired();

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(DomainEntity).IsAssignableFrom(entityType.ClrType))
                modelBuilder.Entity(entityType.Name)
                    .Property<DateTime>("CreatedOn").HasDefaultValueSql("GETDATE()");
        }

        base.OnModelCreating(modelBuilder);
    }
}
