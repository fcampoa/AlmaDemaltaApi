using AlmaDeMalta.api.Responses;
using AlmaDeMalta.api.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
namespace AlmaDeMalta.api.Endpoints.Sales.Dashboard;
public class GetSaleDahboardByIdEndpoint(ISaleDashboardService saleDashboardService) : EndpointWithoutRequest<Results<Ok<Response>, NotFound, BadRequest>>
{
    public override void Configure()
    {
        Get("sales/dashboard/{id}");
        Description(x => x
            .WithName("Get Sale Dashboard By Id")
            .Produces<Response>(200)
            .Produces<Response>(404)
            .Produces<Response>(400)
            .Produces<Response>(500)
            .WithTags("Sales"));
    }
    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<Guid>("id");
        var response = await saleDashboardService.GetByIdAsync(id);
        if (response.Status == System.Net.HttpStatusCode.NotFound)
        {
            await SendResultAsync(TypedResults.NotFound());
            return;
        }
        if (response.Status != System.Net.HttpStatusCode.OK)
        {
            await SendResultAsync(TypedResults.BadRequest(response));
            return;
        }
        await SendResultAsync(TypedResults.Ok(response));
    }
}