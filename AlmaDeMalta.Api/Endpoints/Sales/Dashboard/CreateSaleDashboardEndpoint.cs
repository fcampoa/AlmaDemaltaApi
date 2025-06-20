using AlmaDeMalta.api.Responses;
using AlmaDeMalta.api.Services;
using AlmaDeMalta.Common.Contracts.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
namespace AlmaDeMalta.api.Endpoints.Sales.Dashboard;
public class CreateSaleDashboardEndpoint(ISaleDashboardService saleDashboardService) : Endpoint<SaleDashboard, Results<Ok<Response>, BadRequest>>
{
    public override void Configure()
    {
        Post("sales/dashboard");
        AllowAnonymous();
        Description(x => x
            .WithName("Create Sale Dashboard")
            .Produces<Response>(201)
            .Produces<Response>(400)
            .Produces<Response>(500)
            .WithTags("Sales"));
    }
    public override async Task HandleAsync(SaleDashboard req, CancellationToken ct)
    {
        var response = await saleDashboardService.CreateAsync(req);
        if (response.Status != System.Net.HttpStatusCode.Created)
        {
            await SendResultAsync(TypedResults.BadRequest(response));
            return;
        }
        await SendAsync(TypedResults.Ok(response));
    }
}
