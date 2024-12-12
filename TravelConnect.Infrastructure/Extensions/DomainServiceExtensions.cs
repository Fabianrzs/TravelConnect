using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TravelConnect.Domain.Services;

namespace TravelConnect.Infrastructure.Extensions;
public static class ServiceExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        var _services = new List<Type>();

        Assembly assembly = Assembly.Load(AppConstants.DomainProject);

        _services.AddRange(assembly.GetTypes()
               .Where(p => p.CustomAttributes.Any(x => x.AttributeType
               == typeof(DomainServiceAttribute))));

        _services.ForEach(serviceType => services.AddTransient(serviceType));

        return services;
    }
}