using AlmaDeMalta.api.Responses;
using AlmaDeMalta.api.Services;
using AlmaDeMalta.Common.Contracts.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
namespace AlmaDeMalta.api.Endpoints.Sales.Dashboard;
public class UpdateSaleDashboardEndpoint(ISaleDashboardService saleDashboardService): Endpoint<SaleDashboard, Results<Ok<Response>, BadRequest>>
{
    public override void Configure()
    {
        Put("sales/dashboard/{id}");
        Description(x => x
            .WithName("Update Sale Dashboard")
            .Produces<Response>(200)
            .Produces<Response>(400)
            .Produces<Response>(500)
            .WithTags("Sales"));
    }
    public override async Task HandleAsync(SaleDashboard req, CancellationToken ct)
    {
        var response = await saleDashboardService.UpdateAsync(req);
        if (response.Status != System.Net.HttpStatusCode.OK)
        {
            await SendResultAsync(TypedResults.BadRequest(response));
            return;
        }
        await SendResultAsync(TypedResults.Ok(response));
    }
}
