using AlmaDeMalta.api.Responses;
using AlmaDeMalta.api.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
namespace AlmaDeMalta.api.Endpoints.Sales;
public class GetSalesEndpoint(ISaleService saleService) : EndpointWithoutRequest<Results<Ok<Response>, NotFound>>
{
    public override void Configure()
    {
        Get("sales");
        AllowAnonymous();
        Description(x => x
            .WithName("Get All Sales")
            .Produces<Response>(200)
            .Produces(404)
            .Produces(500)
            .WithTags("Sales"));
    }
    public override async Task HandleAsync(CancellationToken ct)
    {
        var sales = await saleService.GetAllAsync();
        if (sales.Status != System.Net.HttpStatusCode.OK)
        {
            await SendResultAsync(TypedResults.NotFound());
            return;
        }
        await SendResultAsync(TypedResults.Ok(sales));
    }
}
