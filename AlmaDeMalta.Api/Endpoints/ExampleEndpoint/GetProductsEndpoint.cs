namespace AlmaDeMalta.Api.Endpoints;

using AlmaDeMalta.api.Responses;
using AlmaDeMalta.Api.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Net;

public class GetProductsEndpoint(IProductService productService) : EndpointWithoutRequest<Results<Ok<Response>, NotFound>>
{
    public override void Configure()
    {
        Get("products");
        AllowAnonymous();
    }

    public override async Task HandleAsync( CancellationToken ct)
    {
        try
        {
            var products = await productService.GetAllAsync();
            if (products.Status != HttpStatusCode.OK)
            {
                await SendResultAsync(TypedResults.NotFound());
                return;
            }
            await SendResultAsync(TypedResults.Ok(products));
        }
        catch (Exception ex)
        {
            await SendResultAsync(TypedResults.BadRequest(ex.Message));
        }
        
    }
}