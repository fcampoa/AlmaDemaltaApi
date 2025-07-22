using AlmaDeMalta.api.Mappers;
using AlmaDeMalta.api.Requests;
using AlmaDeMalta.api.Responses;
using AlmaDeMalta.api.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
namespace AlmaDeMalta.api.Endpoints.Users;
public class CreateUserEndpoint(IUserService userService) : Endpoint<UserRequest, Results<Ok<Response>, BadRequest>, UserMapper>
{
    public override void Configure()
    {
        Post("users");
        AllowAnonymous();
        Description(x => x
            .WithName("Create User")
            .Produces<Response>(201)
            .Produces<Response>(400)
            .WithTags("Users"));
    }
    public override async Task HandleAsync(UserRequest req, CancellationToken ct)
    {
        var user = Map.ToEntity(req);
        var response = await userService.CreateAsync(user);
        if (response.Status != System.Net.HttpStatusCode.Created)
        {
            await SendResultAsync(TypedResults.BadRequest(response));
            return;
        }
        await SendResultAsync(TypedResults.Ok(response));
    }
}
