using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace AlmaDeMalta.Api;
public static class AuthConfigExtensions
{

    public static IServiceCollection AuthenticationConfig(this IServiceCollection services, WebApplicationBuilder bld)
    {
        if (bld.Environment.IsDevelopment())
        {
            return services.LocalAuth();
        }
        return services.UseAuth0(bld);
    }
    public static IServiceCollection UseAuth0(this IServiceCollection services, WebApplicationBuilder bld)
    {
        services
            .AddAuthorization(
            options =>
            {
                options.AddPolicy(
                    "Default",
                    policy =>
                    {
                        policy.RequireAuthenticatedUser();
                    }
                );
            }
            )
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = $"https://{bld.Configuration["Auth0:Domain"]}/";
                options.Audience = bld.Configuration["Auth0:Audience"];
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new()
                {
                    ValidAudience = bld.Configuration["Auth0:Audience"],
                    ValidIssuer = bld.Configuration["Auth0:Domain"]
                };

                options.Events = new()
                {
                    OnMessageReceived = e =>
                    {
                        Console.WriteLine($"JWT Token Received.");
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = e =>
                    {
                        Console.WriteLine("JWT: token validated");
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = e =>
                    {
                        Console.WriteLine(
                            $"JWT: authentication failed with: {e?.Exception.Message}"
                        );
                        return Task.CompletedTask;
                    },
                    OnChallenge = e =>
                    {
                        Console.WriteLine("JWT: challenge");
                        return Task.CompletedTask;
                    }
                };
            });
        return services;
    }

    public static IServiceCollection LocalAuth(this IServiceCollection services)
    {
        services.AddAuthentication("DummyScheme")
                .AddScheme<AuthenticationSchemeOptions, DummyAuthHandler>("DummyScheme", options => { });
        services.AddAuthorization();
        return services;
    }
}

public class DummyAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public DummyAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
                            ILoggerFactory logger,
                            UrlEncoder encoder,
                            ISystemClock clock)
        : base(options, logger, encoder, clock) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[] { new Claim(ClaimTypes.Name, "LocalUser") };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
