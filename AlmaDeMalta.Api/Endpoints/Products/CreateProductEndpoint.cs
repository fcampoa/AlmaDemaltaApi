using AlmaDeMalta.api.Requests;
using AlmaDeMalta.api.Responses;
using AlmaDeMalta.Api.Services;
using FastEndpoints;

namespace AlmaDeMalta.api.Endpoints.Products;
    public class CreateProductEndpoint(IProductService productService): Endpoint<ProductRequest, Response>
    {
    public override void Configure()
    {
        Post("products");
        AllowAnonymous();
        Description(x => x
            .WithName("Create Product")
            .Produces<Response>(201)
            .Produces<Response>(400)
            .Produces<Response>(500)
            .WithTags("Products"));
    }
    public override async Task HandleAsync(ProductRequest req, CancellationToken ct)
    {       
        var response = await productService.CreateAsync(req);
        await SendAsync(response);
    }
}
