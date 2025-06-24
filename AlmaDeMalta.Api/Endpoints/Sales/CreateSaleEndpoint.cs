using AlmaDeMalta.api.Mappers;
using AlmaDeMalta.api.Requests;
using AlmaDeMalta.api.Responses;
using AlmaDeMalta.api.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
namespace AlmaDeMalta.api.Endpoints.Sales;
public class CreateSaleEndpoint(ISaleService saleService) : Endpoint<SaleRequest, Results<Ok<Response>, BadRequest>, SaleMapper>
{
    public override void Configure()
    {
        Post("sales");
        AllowAnonymous();
        Description(x => x
            .WithName("Create Sale")
            .Produces<Response>(201)
            .Produces<Response>(400)
            .Produces<Response>(500)
            .WithTags("Sales"));
    }
    public override async Task HandleAsync(SaleRequest req, CancellationToken ct)
    {
        var sale = Map.ToEntity(req);
        var response = await saleService.CreateAsync(sale);
        if (response.Status != System.Net.HttpStatusCode.Created)
        {
            await SendResultAsync(TypedResults.BadRequest(response));
            return;
        }
        await SendResultAsync(TypedResults.Ok(response));
    }
}
