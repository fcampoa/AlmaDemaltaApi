using AlmaDeMalta.Common.Contracts.Contracts;

namespace AlmaDeMalta.api.Requests;
    public class ProductSearchRequest
    {
    public ProductType? ProductType { get; set; } = null!;
    public ProductCategory? ProductCategory { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public string? Brand { get; set; } = null;
}
