using AlmaDeMalta.api.Requests;
using AlmaDeMalta.api.Responses;
using AlmaDeMalta.Common.Contracts.Contracts;
using FastEndpoints;
namespace AlmaDeMalta.api.Mappers;
public class SaleMapper : Mapper<SaleRequest, Response, Sale>
{
    public override Sale ToEntity(SaleRequest source)
    {
        return new Sale
        {
            Id = source.Id,
            Subtotal = source.Subtotal,
            Total = source.Total,
            PaymentMethod = source.PaymentMethod,
            Products = source.Products,
            Status = source.Status,
            PurchaseOrderNumberPrefixId = source.PurchaseOrderNumberPrefixId
        };
    }
}
