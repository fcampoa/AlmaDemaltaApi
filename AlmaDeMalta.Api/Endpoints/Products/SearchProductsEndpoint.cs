using AlmaDeMalta.api.Requests;
using AlmaDeMalta.api.Responses;
using AlmaDeMalta.Common.Contracts.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Linq.Expressions;
using System.Net;
using LinqKit;
using AlmaDeMalta.Api.Services;

namespace AlmaDeMalta.api.Endpoints.Products;
    public class SearchProductsEndpoint(IProductService productService): Endpoint<ProductSearchRequest, Results<Ok<Response>, NotFound>>
    {
    public override void Configure()
    {
        Post("products/search");
        Description(x => x
            .WithName("Search Products")
            .Produces<Response>(200)
            .Produces<Response>(404)
            .WithTags("Products"));
    }
    public override async Task HandleAsync(ProductSearchRequest req, CancellationToken ct)
    {
        var predicate = BuildSearchExpression(req);
        var response = await productService.Search(predicate);
        if (response.Status != HttpStatusCode.OK)
        {
            await SendResultAsync(TypedResults.NotFound(response));
            return;
        }
        await SendResultAsync(TypedResults.Ok(response));
    }

    private Expression<Func<Product, bool>> BuildSearchExpression(ProductSearchRequest request)
    {
        var predicate = PredicateBuilder.New<Product>(x => x.isActive == request.IsActive);
        if (request.ProductType != null)
        {
            predicate = predicate.And(x => x.Type == request.ProductType);
        }
        if (request.ProductCategory != null)
        {
            predicate = predicate.And(x => x.Category == request.ProductCategory);
        }
        if (!string.IsNullOrEmpty(request.Brand))
        {
            predicate = predicate.And(x => x.Brand == request.Brand);
        }
        return predicate;
    }
}
