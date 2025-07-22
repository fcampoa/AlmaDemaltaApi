using Serilog;

namespace AlmaDeMalta.api
{
    public static class LoggerConfig
    {
        public static IHostBuilder UseLogging(this IHostBuilder bld)
        {
            return bld.UseSerilog((context, services, configuration) =>
            {
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    .WriteTo.Console();
            });
        }
    }
}
