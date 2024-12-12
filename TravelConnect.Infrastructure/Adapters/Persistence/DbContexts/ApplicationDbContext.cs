using Microsoft.EntityFrameworkCore;
using TravelConnect.Domain.Entities;

namespace TravelConnect.Infrastructure.Adapters.Persistence.DbContexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<EmergencyContact> EmergenciesContacts { get; set; }
    public DbSet<Guest> Guests { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<TravelAgent> TravelsAgents { get; set; }
}
