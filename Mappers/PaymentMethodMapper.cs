using AlmaDeMalta.api.Requests;
using AlmaDeMalta.api.Responses;
using AlmaDeMalta.Common.Contracts.Contracts;
using FastEndpoints;
namespace AlmaDeMalta.api.Mappers;
    public class PaymentMethodMapper: Mapper<PaymentMethodRequest, Response, PaymentMethod>
    {
        public override PaymentMethod ToEntity(PaymentMethodRequest source)
    {
        return new PaymentMethod
        {
            Id = source.Id,
            Name = source.Name,
            Type = source.Type,
            Fee = source.Fee
        };
    }
}
