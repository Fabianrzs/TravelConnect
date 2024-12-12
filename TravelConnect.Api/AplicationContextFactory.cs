using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TravelConnect.Infrastructure.Adapters.Persistence;

namespace TravelConnect.Api;

public class AplicationContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var Config = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
           .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(Config.GetConnectionString("DefaultConnection"), sqlopts =>
        {
            sqlopts.MigrationsHistoryTable("_MigrationHistory", Config.GetValue<string>("SchemaName"));
        });

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
