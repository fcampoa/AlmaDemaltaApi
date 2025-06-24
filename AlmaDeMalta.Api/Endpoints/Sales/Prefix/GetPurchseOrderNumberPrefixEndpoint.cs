using AlmaDeMalta.api.Responses;
using AlmaDeMalta.api.Services;
using AlmaDeMalta.Common.Contracts.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlmaDeMalta.api.Endpoints.Sales.Prefix;
public class GetPurchseOrderNumberPrefixEndpoint(IPurchaseOrderNumberPrefixService _purchaseOrderNumberPrefixService) : EndpointWithoutRequest<Results<Ok<Response>, NotFound>>
{
    public override void Configure()
    {
        Get("sales/prefixes");
        AllowAnonymous();
        Description(x => x
            .WithName("Get Purchase Order Number Prefix")
            .Produces<PurchaseOrderNumberPrefix>(200)
            .Produces<NotFound>(404)
            .WithTags("Sales"));
    }
    public override async Task HandleAsync(CancellationToken ct)
    {
        var response = await _purchaseOrderNumberPrefixService.GetAllAsync();
        if (response.Status == System.Net.HttpStatusCode.NotFound)
        {
            await SendResultAsync(TypedResults.NotFound());
            return;
        }
        await SendResultAsync(TypedResults.Ok(response));
    }
}
