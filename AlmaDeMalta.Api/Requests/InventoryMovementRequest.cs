using AlmaDeMalta.Common.Contracts.Overviews;

namespace AlmaDeMalta.api.Requests;
    public class InventoryMovementRequest
    {
    public Guid Id { get; set; } = Guid.Empty;
    public ProductOvewview Product { get; set; } = null!;
    public decimal Quantity { get; set; } = 0;
    public string? Reason { get; set; } = null!;
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string? UserId { get; set; } = null!;
    public bool IsIncoming { get; set; } = false;
}
