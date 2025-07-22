using AlmaDeMalta.api.Services;
using AlmaDeMalta.Common.Contracts.Contracts;

namespace AlmaDeMalta.api.Middlewares
{
    public class UserLoginMiddleware(RequestDelegate next, ILogger<UserLoginMiddleware> logger)
    {
        public async Task Invoke(HttpContext context, IUserService userService)
        {
            if (context.Request.Path.Equals("/api/users/login", StringComparison.OrdinalIgnoreCase))
            {
                if (context.User.Identity?.IsAuthenticated != true)
                {
                    var auth0Id = context.User.FindFirst("sub")?.Value;
                    var email = context.User.FindFirst("email")?.Value;
                    var name = context.User.FindFirst("name")?.Value;
                    if (!string.IsNullOrEmpty(auth0Id) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(name))
                    {
                        await userService.GetByAuthId(new User
                        {
                            AuthProviderId = auth0Id,
                            Email = email,
                            Name = name
                        });
                    }
                    else
                    {
                        logger.LogWarning("User is authenticated but missing required claims.");
                    }
                }
            }
            await next(context);
        }
    }
}
