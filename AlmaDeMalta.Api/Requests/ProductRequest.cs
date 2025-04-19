using AlmaDeMalta.Common.Contracts.Contracts;

namespace AlmaDeMalta.api.Requests;
    public class ProductRequest: BaseRequest
    {
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public ProductCategory Category { get; set; } = ProductCategory.Other;
    public string ImageUrl { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public ProductType Type { get; set; } = ProductType.Other;
    public MesaureUnit Unit { get; set; } = MesaureUnit.Other;
}
