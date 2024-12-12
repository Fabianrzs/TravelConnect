using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TravelConnect.Domain.Ports.Persistence;
using TravelConnect.Infrastructure.Adapters.Persistence.DbContexts;
using TravelConnect.Infrastructure.Adapters.Persistence.Repositories;

namespace TravelConnect.Infrastructure.Extensions;

public static class PersistenceExtension
{
    public static IServiceCollection AddPesistenceServices(this IServiceCollection svc, IConfiguration config)
    {
        var stringConnection = config.GetConnectionString("DefaultConnection");
        svc.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(stringConnection));

        svc.AddTransient(typeof(IRepository<>), typeof(Repository<>));
        return svc;
    }
}