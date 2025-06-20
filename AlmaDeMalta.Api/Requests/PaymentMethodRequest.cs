using AlmaDeMalta.Common.Contracts.Contracts;

namespace AlmaDeMalta.api.Requests;
    public class PaymentMethodRequest: BaseRequest
    {
    public Guid Id { get; set; } = Guid.Empty;
    public bool IsActive { get; set; } = true;
    public string Name { get; set; } = string.Empty;
    public PaymentType Type { get; set; } = PaymentType.Other;
    public decimal Fee { get; set; } = 0.0m;
}
