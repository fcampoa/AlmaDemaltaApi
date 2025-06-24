using AlmaDeMalta.api.Mappers;
using AlmaDeMalta.api.Requests;
using AlmaDeMalta.api.Responses;
using AlmaDeMalta.api.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;

namespace AlmaDeMalta.api.Endpoints.Storage;
public class CreateInventoryMovementEndpoint(IInventoryMovementsService inventoryMovementsService) : Endpoint<InventoryMovementRequest, Results<Ok<Response>, BadRequest>>
{
    public override void Configure()
    {
        Post("inventory");
        AllowAnonymous();
        Description(x => x
            .WithName("Create Inventory Movement")
            .Produces<Response>(201)
            .Produces<Response>(400)
            .Produces<Response>(500)
            .WithTags("Inventory Movements"));
    }
    public override async Task HandleAsync(InventoryMovementRequest req, CancellationToken ct)
    {
        try {
        var mapper = new InventoryMovementMapper();
        var inventoryMovement = mapper.ToEntity(req);
        var response = await inventoryMovementsService.CreateAsync(inventoryMovement);
        if (response.Status != HttpStatusCode.Created)
        {
            await SendResultAsync(TypedResults.BadRequest(response));
            return;
        }
        await SendResultAsync(TypedResults.Ok(response));
    }
        catch (Exception ex)
        {
            await SendResultAsync(TypedResults.BadRequest(ex.Message));
        }
    }
}
