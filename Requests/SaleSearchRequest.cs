using AlmaDeMalta.Common.Contracts.Contracts;

namespace AlmaDeMalta.api.Requests
{
    public class SaleSearchRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public StatusEnum? Status { get; set; }
        public Guid? ProductId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
