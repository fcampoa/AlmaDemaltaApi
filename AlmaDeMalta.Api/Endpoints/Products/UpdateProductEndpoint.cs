using AlmaDeMalta.api.Mappers;
using AlmaDeMalta.api.Requests;
using AlmaDeMalta.api.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using FastEndpoints;
using AlmaDeMalta.Api.Services;

namespace AlmaDeMalta.api.Endpoints.Products;
    public class UpdateProductEndpoint(IProductService productService): Endpoint<ProductRequest, Results<Ok<Response>, BadRequest>, ProductMapper>
    {
        public override void Configure()
        {
            Put("products/{id}");
            AllowAnonymous();
            Description(x => x
                .WithName("Update Product")
                .Produces<Response>(StatusCodes.Status200OK)
                .Produces<Response>(StatusCodes.Status404NotFound)
                .Produces<Response>(StatusCodes.Status400BadRequest));
        }
        public override async Task HandleAsync(ProductRequest request, CancellationToken ct)
        {
            var product = Map.ToEntity(request);
            var response = await productService.UpdateAsync(product);
            await SendResultAsync(response.IsSuccess ? TypedResults.Ok(response) : TypedResults.BadRequest(response));
    }
    }
