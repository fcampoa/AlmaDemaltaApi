namespace AlmaDeMalta.Api.Endpoints;
using FastEndpoints;

public class ExampleEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/example");
        AllowAnonymous();
    }

    public override async Task HandleAsync( CancellationToken ct)
    {
        var response = new
        {
            Message = "Hello, World!"
        };

        await SendAsync(response, cancellation: ct);
    }
}