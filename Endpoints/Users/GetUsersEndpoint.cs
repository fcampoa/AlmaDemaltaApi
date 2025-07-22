using AlmaDeMalta.api.Requests;
using AlmaDeMalta.api.Responses;
using AlmaDeMalta.api.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlmaDeMalta.api.Endpoints.Users;
public class GetUsersEndpoint(IUserService userService) : EndpointWithoutRequest<Results<Ok<Response>, NotFound>>
{
    public override void Configure()
    {
        Get("users");
        Description(x => x
            .WithName("Get Users")
            .Produces<Response>(200)
            .Produces<Response>(404)
            .WithTags("Users"));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var response = await userService.GetAllAsync();
        if (!response.IsSuccess)
        {
            await SendResultAsync(TypedResults.NotFound(response));
            return;
        }
        await SendResultAsync(TypedResults.Ok(response));
    }
}

