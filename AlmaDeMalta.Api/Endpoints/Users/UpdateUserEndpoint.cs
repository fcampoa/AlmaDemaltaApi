using AlmaDeMalta.api.Mappers;
using AlmaDeMalta.api.Requests;
using AlmaDeMalta.api.Responses;
using AlmaDeMalta.api.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
namespace AlmaDeMalta.api.Endpoints.Users;
public class UpdateUserEndpoint(IUserService userService) : Endpoint<UserRequest, Results<Ok<Response>, BadRequest>, UserMapper>
{
    public override void Configure()
    {
        Put("users/{id}");
        Description(x => x
            .WithName("Update User")
            .Produces<Response>(200)
            .Produces<Response>(400)
            .WithTags("Users"));
    }
    public override async Task HandleAsync(UserRequest req, CancellationToken ct)
    {
        // Assuming you have a userService to handle the update logic
        var user = Map.ToEntity(req);
        var response = await userService.UpdateAsync(user);
        if (response.Status != System.Net.HttpStatusCode.OK)
        {
            await SendResultAsync(TypedResults.BadRequest(response));
            return;
        }
        await SendResultAsync(TypedResults.Ok(response));
    }
}
