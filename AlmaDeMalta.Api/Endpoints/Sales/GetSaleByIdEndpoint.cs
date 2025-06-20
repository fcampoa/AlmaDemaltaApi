using AlmaDeMalta.api.Responses;
using AlmaDeMalta.api.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
namespace AlmaDeMalta.api.Endpoints.Sales;
public class GetSaleByIdEndpointd(ISaleService saleService) : EndpointWithoutRequest<Results<Ok<Response>, NotFound>>
{
    public override void Configure()
    {
        Get("sales/{id}");
        AllowAnonymous();
        Description(x => x
            .WithName("Get Sale By Id")
            .Produces<Response>(200)
            .Produces(404)
            .Produces(500)
            .WithTags("Sales"));
    }
    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<Guid>("id");
        var sale = await saleService.GetByIdAsync(id);
        if (sale.Status != System.Net.HttpStatusCode.OK)
        {
            await SendResultAsync(TypedResults.NotFound());
            return;
        }
        await SendResultAsync(TypedResults.Ok(sale));
    }
}
