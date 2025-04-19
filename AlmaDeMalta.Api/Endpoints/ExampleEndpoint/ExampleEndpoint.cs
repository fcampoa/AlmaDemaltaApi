namespace AlmaDeMalta.Api.Endpoints;

using AlmaDeMalta.Api.Services;
using FastEndpoints;

public class ExampleEndpoint(IProductService productService) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/example");
        AllowAnonymous();
    }

    public override async Task HandleAsync( CancellationToken ct)
    {
        var products = await productService.GetAllAsync();
        await SendAsync(products, cancellation: ct);
    }
}