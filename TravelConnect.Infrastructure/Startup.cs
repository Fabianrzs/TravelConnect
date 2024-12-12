using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TravelConnect.Infrastructure.Extensions;

namespace TravelConnect.Infrastructure;

public static class Startup
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddPesistenceServices(config);
        services.AddDomainServices();
        services.AddSecurityServices(config);
        services.AddMapperServices();
        services.AddSwaggerServices();
        services.AddCorsPolicyServices();
        services.AddControllers();
    }

    public static void UseInfrastructure(this WebApplication app, IWebHostEnvironment env)
    {
        app.UseCorsPolicyApp();
        app.UseSwaggerApp(env);
        app.UseExceptionMiddlewareApp();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseHttpsRedirection();
        app.MapControllers();
    }
}