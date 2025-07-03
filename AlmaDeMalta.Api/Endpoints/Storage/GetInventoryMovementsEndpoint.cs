using AlmaDeMalta.api.Responses;
using AlmaDeMalta.api.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlmaDeMalta.api.Endpoints.Storage;
public class GetInventoryMovementsEndpoint(IInventoryMovementsService inventoryMovementsService): EndpointWithoutRequest<Results<Ok<Response>, NotFound>>
{
    public override void Configure()
    {
        Get("inventory/movements");
        Description(x => x
            .WithName("Get Inventory Movements")
            .Produces<Response>(200)
            .Produces<Response>(404)
            .WithTags("Inventory"));
    }
    public override async Task HandleAsync(CancellationToken ct)
    {
        var response = await inventoryMovementsService.GetAllAsync();
        if (response.Status != System.Net.HttpStatusCode.OK)
        {
            await SendResultAsync(TypedResults.NotFound(response));
            return;
        }
        await SendResultAsync(TypedResults.Ok(response));
    }
}
