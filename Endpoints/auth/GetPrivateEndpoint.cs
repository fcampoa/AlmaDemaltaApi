using AlmaDeMalta.api.Responses;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlmaDeMalta.api.Endpoints.auth;
public class GetPrivateDataEndpoint : EndpointWithoutRequest<Results<Ok<Response>, NotFound>>
{
    public override void Configure()
    {
        Get("private");
        Summary(s => s.Summary = "Obtiene datos privados - requiere autenticación");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendResultAsync(TypedResults.Ok(AlmaDeMalta.api.Responses.Response.Success("Autorizado")));
    }
}
