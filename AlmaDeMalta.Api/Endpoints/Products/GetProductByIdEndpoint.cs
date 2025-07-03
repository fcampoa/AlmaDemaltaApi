using AlmaDeMalta.api.Responses;
using AlmaDeMalta.Api.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;
namespace AlmaDeMalta.api.Endpoints.Products
{
    public class GetProductByIdEndpoint(IProductService productService) : EndpointWithoutRequest<Results<Ok<Response>, NotFound>>
    {
        public override void Configure()
        {
            Get("products/{id}");
            Description(x => x
                .WithName("Get Product By Id")
                .Produces<Response>(200)
                .Produces(404)
                .Produces(500)
                .WithTags("Products"));
        }
        public override async Task HandleAsync(CancellationToken ct)
        {
            var id = Route<Guid>("id");
            var product = await productService.GetByIdAsync(id);
            if (product.Status != HttpStatusCode.OK)
            {
                await SendResultAsync(TypedResults.NotFound());
                return;
            }
            await SendResultAsync(TypedResults.Ok(product));
        }
    }
}
