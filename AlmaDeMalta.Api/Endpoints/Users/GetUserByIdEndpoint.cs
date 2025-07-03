using AlmaDeMalta.api.Requests;
using AlmaDeMalta.api.Responses;
using AlmaDeMalta.api.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
namespace AlmaDeMalta.api.Endpoints.Users;
public class GetUserByIdEndpoint(IUserService userService) : EndpointWithoutRequest<Results<Ok<Response>, NotFound>>
{
    public override void Configure()
    {
        Get("users/{id}");
        Description(x => x
            .WithName("Get User By Id")
            .Produces<Response>(200)
            .Produces<Response>(404)
            .WithTags("Users"));
    }
    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = Route<Guid>("id");
        var response = await userService.GetByIdAsync(userId);
        if (response.Status != System.Net.HttpStatusCode.OK)
        {
            await SendResultAsync(TypedResults.NotFound(response));
            return;
        }
        await SendResultAsync(TypedResults.Ok(response));
    }
}
