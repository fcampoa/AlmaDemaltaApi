using AlmaDeMalta.api.Mappers;
using AlmaDeMalta.api.Requests;
using AlmaDeMalta.api.Responses;
using AlmaDeMalta.api.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
namespace AlmaDeMalta.api.Endpoints.PaymentMethods;
public class CreatePaymentMethodEndpoint(IPaymentMethodService paymentMethodService) : Endpoint<PaymentMethodRequest, Results<Ok<Response>, BadRequest>, PaymentMethodMapper>
{
    public override void Configure()
    {
        Post("payment-methods");
        AllowAnonymous();
        Description(x => x
            .WithName("Create Payment Method")
            .Produces<Response>(201)
            .Produces<Response>(400)
            .Produces<Response>(500)
            .WithTags("Payment Methods"));
    }
    public override async Task HandleAsync(PaymentMethodRequest req, CancellationToken ct)
    {
        var paymentMethod = Map.ToEntity(req);
        var response = await paymentMethodService.CreateAsync(paymentMethod);
        if (response.Status != System.Net.HttpStatusCode.Created)
        {
            await SendResultAsync(TypedResults.BadRequest(response));
            return;
        }
        await SendResultAsync(TypedResults.Ok(response));
    }
}
