using AlmaDeMalta.api.Responses;
using AlmaDeMalta.api.Services;
using AlmaDeMalta.Common.Contracts.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
namespace AlmaDeMalta.api.Endpoints.Sales.Prefix;
public class CreatePurchaseOrderNumberPrefix(IPurchaseOrderNumberPrefixService _purchaseOrderNumberPrefixService) : Endpoint<PurchaseOrderNumberPrefix, Results<Ok<Response>, BadRequest>>
{
    public override void Configure()
    {
        Post("sales/prefixes");
        AllowAnonymous();
        Description(x => x
            .WithName("Create Purchase Order Number Prefix")
            .Produces<Response>(201)
            .Produces<Response>(400)
            .Produces<Response>(500)
            .WithTags("Sales"));
    }
    public override async Task HandleAsync(PurchaseOrderNumberPrefix req, CancellationToken ct)
    {
        var response = await _purchaseOrderNumberPrefixService.CreateAsync(req);
        if (response.Status != System.Net.HttpStatusCode.Created)
        {
            await SendResultAsync(TypedResults.BadRequest(response));
        }
        else
        {
            await SendResultAsync(TypedResults.Ok(response));
        }
    }
}
