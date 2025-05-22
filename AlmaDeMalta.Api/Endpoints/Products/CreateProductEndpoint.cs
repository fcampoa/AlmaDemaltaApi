using AlmaDeMalta.api.Mappers;
using AlmaDeMalta.api.Requests;
using AlmaDeMalta.api.Responses;
using AlmaDeMalta.Api.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;

namespace AlmaDeMalta.api.Endpoints.Products;
    public class CreateProductEndpoint(IProductService productService): Endpoint<ProductRequest, Results<Ok<Response>, BadRequest>>
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
        var mapper = new ProductMapper();
        var product = mapper.ToEntity(req);
        var response = await productService.CreateAsync(product);
        if (response.Status != HttpStatusCode.Created)
        {
            await SendResultAsync(TypedResults.BadRequest(response));
            return;
        }
        await SendAsync(TypedResults.Ok(response));
    }
}
