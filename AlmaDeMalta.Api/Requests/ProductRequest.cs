using AlmaDeMalta.Common.Contracts.Contracts;
using System.Runtime.CompilerServices;

namespace AlmaDeMalta.api.Requests;
    public class ProductRequest: BaseRequest
    {
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; } = 0.0m;
    public ProductCategory Category { get; set; } = ProductCategory.Other;
    public string ImageUrl { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public ProductType Type { get; set; } = ProductType.Other;
    public MesaureUnit Unit { get; set; } = MesaureUnit.Other;
    public decimal Stock { get; set; } = 0.0m;
    public decimal Availability { get; set; } = 0.0m;
    public string Barcode { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
