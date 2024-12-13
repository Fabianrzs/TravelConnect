using Microsoft.Extensions.DependencyInjection;

namespace TravelConnect.Infrastructure.Extensions;

public static class AutoMapperExtensions
{
    public static IServiceCollection AddMapperServices(this IServiceCollection services)
    {
        services.AddAutoMapper(AppAssembly.InfrastructureAssembly);
        return services;
    }
}