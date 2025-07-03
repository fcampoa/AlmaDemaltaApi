using AlmaDeMalta.api.Mappers;
using AlmaDeMalta.api.Requests;
using AlmaDeMalta.api.Responses;
using AlmaDeMalta.api.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
namespace AlmaDeMalta.api.Endpoints.auth;
public class LoginEndpoint(IUserService userService) : Endpoint<UserRequest, Results<Ok<Response>, BadRequest>, UserMapper>
{
    public override void Configure()
    {
        Post("users/login");
        AllowAnonymous();
        Description(x => x
            .WithName("Login")
            .Produces<Response>(200)
            .Produces<Response>(400)
            .WithTags("Auth"));
    }
    public override async Task HandleAsync(UserRequest req, CancellationToken ct)
    {
        var user = Map.ToEntity(req);
        var response = await userService.GetByAuthId(user);
        if (response.Status != System.Net.HttpStatusCode.OK)
        {
            await SendResultAsync(TypedResults.BadRequest(response));
            return;
        }
        await SendResultAsync(TypedResults.Ok(response));
    }
}
