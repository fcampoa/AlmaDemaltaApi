using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;

namespace AlmaDeMalta.api.Endpoints;
    public class SandBox: EndpointWithoutRequest<Results<Ok<string>, NotFound>>
    {
    public override void Configure()
    {
        Get("sandbox");
        AllowAnonymous();
        Description(x => x
            .WithName("Sandbox Endpoint")
            .Produces<string>(200)
            .Produces<NotFound>(404)
            .WithTags("Sandbox"));
    }
    public override async Task HandleAsync(CancellationToken ct)
    {
        // Simulate some processing
        await Task.Delay(1000, ct);
        await SendResultAsync(TypedResults.Ok("Sandbox is operational! :D :D test"));
    }
}
