using AlmaDeMalta.api.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
namespace AlmaDeMalta.api.Endpoints.PaymentMethods;
    public class GetPaymentMethodsEndpoint(IPaymentMethodService paymentMethodService): EndpointWithoutRequest<Results<Ok, NotFound>>
    {
    public override void Configure()
    {
        Get("payment-methods");
        AllowAnonymous();
        Description(x => x
            .WithName("Get All Payment Methods")
            .Produces(200)
            .Produces(404)
            .Produces(500)
            .WithTags("Payment Methods"));
    }
    public override async Task HandleAsync(CancellationToken ct)
    {
        var paymentMethods = await paymentMethodService.GetAllAsync();
        if (paymentMethods.Status != System.Net.HttpStatusCode.OK)
        {
            await SendResultAsync(TypedResults.NotFound());
            return;
        }
        await SendResultAsync(TypedResults.Ok(paymentMethods));
    }
}
