using AlmaDeMalta.Common.Contracts.Contracts;
using AlmaDeMalta.Common.Contracts.Overviews;

namespace AlmaDeMalta.api.Requests;
    public class SaleRequest: BaseRequest
    {
    public Guid Id { get; set; } = Guid.Empty;
    public bool IsActive { get; set; } = true;
    public List<SaleDetail> Products { get; set; } = [];
    public decimal Subtotal { get; set; } = 0;
    public decimal Total { get; set; } = 0;
    public PaymentMethod PaymentMethod { get; set; } = null!;
    public StatusEnum Status { get; set; } = StatusEnum.Draft;
    public Guid? PurchaseOrderNumberPrefixId { get; set; } = null!;
}
