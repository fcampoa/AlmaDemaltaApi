using AlmaDeMalta.api.Services;
using AlmaDeMalta.Common.Contracts.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
namespace AlmaDeMalta.api.Endpoints.auth;
public class SyncUsersEndpoint(IAuth0Service auth0Service, IUserService userService) : EndpointWithoutRequest<Results<Ok, BadRequest>>
{
    public override void Configure()
    {
        Post("auth/sync");
        AllowAnonymous();
        Description(x => x
            .WithName("Sync Users")
            .Produces(200)
            .Produces(400)
            .WithTags("Auth"));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var authUsers = await auth0Service.SyncUsers();
        foreach (var user in authUsers)
        {
            var existingUser = await userService.Search(u => u.AuthProviderId == user.user_id || u.Email ==  user.email);
            if (!existingUser.IsSuccess)
            {
                var newUser = new User
                {
                    AuthProviderId = user.user_id,
                    Email = user.email,
                    Name = user.name,
                };
                await userService.CreateAsync(newUser);
            }

        }
        await SendResultAsync(TypedResults.Ok());
    }
}
