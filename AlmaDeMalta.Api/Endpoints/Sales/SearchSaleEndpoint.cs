using AlmaDeMalta.api.Requests;
using AlmaDeMalta.api.Responses;
using AlmaDeMalta.api.Services;
using AlmaDeMalta.Common.Contracts.Contracts;
using FastEndpoints;
using LinqKit;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Linq.Expressions;
namespace AlmaDeMalta.api.Endpoints.Sales;
public class SearchSaleEndpoint(ISaleService saleService) : Endpoint<SaleSearchRequest, Results<Ok<Response>, NotFound>>
{
    public override void Configure()
    {
        Post("sales/search");
        AllowAnonymous();
        Description(x => x
            .WithName("Search Sales")
            .Produces<Response>(200)
            .Produces(404)
            .Produces(500)
            .WithTags("Sales"));
    }
    public override async Task HandleAsync(SaleSearchRequest req, CancellationToken ct)
    {
        var response = await saleService.Search(BuildSearchExpression(req));
        if (response.Status != System.Net.HttpStatusCode.OK)
        {
            await SendResultAsync(TypedResults.NotFound());
            return;
        }
        await SendResultAsync(TypedResults.Ok(response));
    }
    private Expression<Func<Sale, bool>> BuildSearchExpression(SaleSearchRequest request)
    {
        Expression<Func<Sale, bool>> expression = PredicateBuilder.New<Sale>(sale => sale.isActive == request.IsActive); // Start with a true condition
        if (request.ProductId != null)
        {
            expression = expression.And(sale => sale.Products.Any(p => p.Product.ProductId == request.ProductId));
        }
        if (request.StartDate.HasValue)
        {
            expression = expression.And(sale => sale.CreatedAt >= request.StartDate.Value);
        }
        if (request.EndDate.HasValue)
        {
            expression = expression.And(sale => sale.CreatedAt <= request.EndDate.Value);
        }
        if (request.Status != null)
        {
            expression = expression.And(sale => sale.Status == request.Status);
        }
        return expression;
    }
}
