using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TravelConnect.Domain.Ports.Persistence;
using TravelConnect.Infrastructure.Adapters.Persistence;
using TravelConnect.Infrastructure.Persistence;

namespace TravelConnect.Infrastructure.Extensions;

public static class PersistenceExtension
{
    public static IServiceCollection AddPesistenceServices(this IServiceCollection svc, IConfiguration config)
    {
        var user = config["TravelConnect.DbConections-UserId"];
        var password = config["TravelConnect.DbConections-Password"];
        var server = config["TravelConnect.DbConections-Server"];
        var database = config["TravelConnect.DbConections-Database"];

        var stringConnection = config.GetConnectionString("DefaultConnection")!
            .Replace("{user}", user).Replace("{password}",password)
            .Replace("{server}",server).Replace("{database}",database);

        svc.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(stringConnection));
        
        svc.AddScoped<IUnitOfWork, UnitOfWork>();

        svc.AddTransient(typeof(IRepository<>), typeof(Repository<>));
        return svc;
    }
}