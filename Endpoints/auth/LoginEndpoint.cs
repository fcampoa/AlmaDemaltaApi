using AlmaDeMalta.api.Mappers;
using AlmaDeMalta.api.Requests;
using AlmaDeMalta.api.Responses;
using AlmaDeMalta.api.Services;
using AlmaDeMalta.Common.Contracts.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;
namespace AlmaDeMalta.api.Endpoints.auth;
public class LoginEndpoint(IUserService userService) : EndpointWithoutRequest<Results<Ok<Response>, BadRequest>>
{
    public override void Configure()
    {
        Post("users/login");
        // AllowAnonymous();
        Description(x => x
            .WithName("Login")
            .Produces<Response>(200)
            .Produces<Response>(400)
            .WithTags("Auth"));
    }
    public override async Task HandleAsync(CancellationToken ct)
    {
        var authId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var response = await userService.FindOne(u => u.AuthProviderId == authId);
        if (response.Status != System.Net.HttpStatusCode.OK)
        {
            await SendResultAsync(TypedResults.BadRequest(response));
            return;
        }
        await SendResultAsync(TypedResults.Ok(response));
    }
}
