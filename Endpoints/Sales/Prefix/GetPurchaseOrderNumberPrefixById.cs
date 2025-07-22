using AlmaDeMalta.api.Responses;
using AlmaDeMalta.api.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
namespace AlmaDeMalta.api.Endpoints.Sales.Prefix;
public class GetPurchaseOrderNumberPrefixById(IPurchaseOrderNumberPrefixService purchaseOrderNumberPrefixService): EndpointWithoutRequest<Results<Ok<Response>, NotFound>>
{
    public override void Configure()
    {
        Get("sales/prefixes/{id}");
        Description(x => x
            .WithName("Get Purchase Order Number Prefix By Id")
            .Produces<Response>(200)
            .Produces<NotFound>(404)
            .WithTags("Sales"));
    }
    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<Guid>("id");
        var response = await purchaseOrderNumberPrefixService.GetByIdAsync(id);
        if (response.Status == System.Net.HttpStatusCode.NotFound)
        {
            await SendResultAsync(TypedResults.NotFound());
            return;
        }
        await SendResultAsync(TypedResults.Ok(response));
    }
}
