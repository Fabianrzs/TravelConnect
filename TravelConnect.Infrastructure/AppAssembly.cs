using System.Reflection;

namespace TravelConnect.Infrastructure;

public static class AppAssembly
{

    public static Assembly ApplicationAssembly { get; set; } = Assembly.Load(AppConstants.ApplicationProject);
    public static Assembly InfrastructureAssembly { get; set; } = Assembly.Load(AppConstants.InfrastructureProject);
    public static Assembly ApiAssembly { get; set; } = Assembly.Load(AppConstants.ApiProject);
    public static Assembly DomainAssembly { get; set; } = Assembly.Load(AppConstants.DomainProject);
}