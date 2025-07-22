using AlmaDeMalta.api.Requests;
using AlmaDeMalta.api.Responses;
using AlmaDeMalta.Common.Contracts.Contracts;
using FastEndpoints;

namespace AlmaDeMalta.api.Mappers;
    public class InventoryMovementMapper: Mapper<InventoryMovementRequest, Response, InventoryMovements>
    {
        public override InventoryMovements ToEntity(InventoryMovementRequest source)
        {
            return new InventoryMovements
            {
                Id = source.Id,
                Product = source.Product,
                Quantity = source.Quantity,
                Reason = source.Reason,
                Date = source.Date,
                UserId = source.UserId,
                IsIncoming = source.IsIncoming
            };
        }
}
