using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AlmaDeMalta.api;
public static class AuthConfig
{
    public static IServiceCollection UseAuth0(this IServiceCollection services, IConfiguration configuration)
    {
        var auth0Settings = configuration.GetSection("Auth0");
        if (auth0Settings == null)
        {
            throw new ArgumentNullException(nameof(auth0Settings), "Auth0 settings not found in configuration.");
        }
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer("Auth0", options =>
        {
            options.Authority = auth0Settings["Authority"];
            options.Audience = auth0Settings["Audience"];
            options.RequireHttpsMetadata = true;
        });
        return services;
    }
}
