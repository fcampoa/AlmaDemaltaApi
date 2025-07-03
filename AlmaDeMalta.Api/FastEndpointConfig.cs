using FastEndpoints;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AlmaDeMalta.api;
public static class FastEndpointConfig
{
    public static IApplicationBuilder FastEndpointSetup(this IApplicationBuilder app)
    {
      return  app.UseFastEndpoints(x =>
        {
            x.Endpoints.RoutePrefix = "api";
            x.Serializer.Options.PropertyNamingPolicy = null;
        });
    }
}
