using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TravelConnect.Infrastructure.Extensions;

namespace TravelConnect.Infrastructure;

public static class Startup
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddPesistenceServices(configuration);
        services.AddDomainServices();
        services.AddSecurityServices(configuration);
        services.AddMapperServices();
        services.AddSwaggerServices();
        services.AddCorsPolicyServices();
        services.AddControllers();
    }

    public static void UseInfrastructure(this WebApplication app)
    {
        app.UseCorsPolicyApp();
        app.UseSwaggerApp();
        app.UseExceptionMiddlewareApp();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseHttpsRedirection();
        app.MapControllers();
    }
}