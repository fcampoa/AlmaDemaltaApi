using AlmaDeMalta.api.Services.Impl;
using AlmaDeMalta.Common.Contracts.Attributes;
using System.Reflection;

namespace AlmaDeMalta.Common.Services;
public static class ServicesExtensionMethods
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        var types = Assembly.GetExecutingAssembly()
                             .GetTypes()
                             .Where(t => !t.IsClass && t.GetCustomAttribute<ServiceClass>() != null)
                             .Select(t => new
                                {
                                    Service = t,
                                    Implementation = t.GetCustomAttribute<ServiceClass>()?.TargetClass,
                                    StrategyEnum = t.GetCustomAttribute<ServiceClass>()?.Strategy
                                })
                             .ToList();
        foreach (var service in types)
        {
            if (service.Implementation == null) { continue; }
            _ = service.StrategyEnum switch
            {
                StrategyEnum.Singleton => services.AddSingleton(service.Service, service.Implementation),
                StrategyEnum.Transient => services.AddTransient(service.Service, service.Implementation),
                StrategyEnum.Scoped => services.AddScoped(service.Service, service.Implementation),
                _ => services
            };
        }
        return services;
    }

    public static IServiceCollection RegisterUtilities(this IServiceCollection services)
    {
        services.AddScoped<ConversionService>();

        return services;
    }
}
