using AlmaDeMalta.api.Responses;
using AlmaDeMalta.api.Services;
using AlmaDeMalta.Common.Contracts.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
namespace AlmaDeMalta.api.Endpoints.Sales.Prefix;
public class UpdatePurchaseOrderNumberPrefix(IPurchaseOrderNumberPrefixService purchaseOrderNumberPrefixService) : Endpoint<PurchaseOrderNumberPrefix, Results<Ok<Response>, BadRequest>>
{
    public override void Configure()
    {
        Put("sales/prefixes/{id}");
        Description(x => x
            .WithName("Update Purchase Order Number Prefix")
            .Produces<Response>(StatusCodes.Status200OK)
            .Produces<Response>(StatusCodes.Status400BadRequest)
            .WithTags("Sales"));
    }
    public override async Task HandleAsync(PurchaseOrderNumberPrefix req, CancellationToken ct)
    {
        var response = await purchaseOrderNumberPrefixService.UpdateAsync(req);
        if (response.Status != System.Net.HttpStatusCode.OK)
        {
            await SendResultAsync(TypedResults.BadRequest(response));
        }
        else
        {
            await SendResultAsync(TypedResults.Ok(response));
        }
    }
}
