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
            if (service.Implementation == null) {  continue; }
            switch(service.StrategyEnum)
            {
                case StrategyEnum.Singleton:
                    services.AddSingleton(service.Service, service.Implementation);
                    break;
                case StrategyEnum.Transient:
                    services.AddTransient(service.Service, service.Implementation);
                    break;
                case StrategyEnum.Scoped:
                    services.AddScoped(service.Service, service.Implementation);
                    break;
            }
        }

        return services;
    }

    public static IServiceCollection RegisterUtilities(this IServiceCollection services)
    {
        services.AddScoped<ConversionService>();

        return services;
    }
}
