using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;


namespace TravelConnect.Infrastructure.Extensions;

public static class CorsExtensions
{
    private static readonly string corsPolicy = "CorsPolicy";
    public static IServiceCollection AddCorsPolicyServices(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(name: corsPolicy,
                   builder =>
                   {
                       builder.AllowAnyOrigin();
                       builder.AllowAnyMethod();
                       builder.AllowAnyHeader();
                   });
        });
        return services;
    }

    public static IApplicationBuilder UseCorsPolicyApp(this IApplicationBuilder app)
    {
        app.UseCors(corsPolicy);
        return app;
    }
}