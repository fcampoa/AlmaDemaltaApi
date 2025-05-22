using AlmaDeMalta.api.Mappers;
using AlmaDeMalta.api.Requests;
using AlmaDeMalta.api.Responses;
using AlmaDeMalta.Common.Contracts.Contracts;
using AlmaDeMalta.Common.Contracts.DataBase;
using System.Net;

namespace AlmaDeMalta.api.Services.Impl
{
    public class InventoryMovementsService(IAlmaDeMaltaUnitOfWork unitOfWork) : IInventoryMovementsService
    {
        private readonly string SuccessCreateMessage = "Inventory movement created successfully.";
        private readonly string SuccessGetAllMessage = "Inventory movements retrieved successfully.";
        private readonly string SuccessGetByIdMessage = "Inventory movement retrieved successfully.";
        private readonly string SuccessUpdateMessage = "Inventory movement updated successfully.";
        private readonly string SuccessDeleteMessage = "Inventory movement deleted successfully.";
        private readonly string NotFoundMessage = "Inventory movement not found.";
        private readonly string InvalidInventoryMovementNotFoundMessage = "Inventory movement not found with the given ID.";
        private readonly string InvalidInventoryMovementDeleteMessage = "Invalid inventory movement ID for deletion.";
        public async Task<Response> CreateAsync(InventoryMovements entity)
        {
            try
            {
                entity.Id = Guid.NewGuid();
                await unitOfWork.InventoryMovementsRepository.CreateAsync(entity);

                var productsRepo = unitOfWork.ProductRepository;
                var product = await productsRepo.FindOneAsync(x => x.Id == entity.Product.ProductId);
                if (product == null)
                {
                    return Response.NotFound(InvalidInventoryMovementNotFoundMessage);
                }
                product.Stock += entity.IsIncoming ? entity.Quantity : -entity.Quantity;

                await productsRepo.UpdateAsync(x => x.Id == product.Id, product);

                return Response.Success(entity, SuccessCreateMessage, HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return Response.ServerError(ex.Message);
            }
        }

        public async Task<Response> DeleteAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return Response.Error(InvalidInventoryMovementDeleteMessage);
                }
                var repo = unitOfWork.InventoryMovementsRepository;
                var inventoryMovement = await repo.FindOneAsync(x => x.Id == id);
                if (inventoryMovement == null)
                {
                    return Response.NotFound(NotFoundMessage);
                }
                await repo.DeleteAsync(x => x.Id == id);
                return Response.Success(id, SuccessDeleteMessage);
            }
            catch (Exception ex)
            {
                return Response.ServerError(ex.Message);
            }
        }

        public async Task<Response> GetAllAsync()
        {
            try
            {
                var repo = unitOfWork.InventoryMovementsRepository;
                var inventoryMovements = await repo.GetAsync();
                if (inventoryMovements == null || !inventoryMovements.Any())
                {
                    return Response.NotFound(NotFoundMessage);
                }
                var response = inventoryMovements;
                return Response.Success(response, SuccessGetAllMessage, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Response.ServerError(ex.Message);
            }
        }

        public async Task<Response> GetByIdAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return Response.Error(InvalidInventoryMovementNotFoundMessage);
                }
                var repo = unitOfWork.InventoryMovementsRepository;
                var inventoryMovement = await repo.FindOneAsync(x => x.Id == id);
                if (inventoryMovement == null)
                {
                    return Response.NotFound(NotFoundMessage);
                }
                var response = inventoryMovement;
                return Response.Success(response, SuccessGetByIdMessage);
            }
            catch (Exception ex)
            {
                return Response.ServerError(ex.Message);
            }
        }

        public async Task<Response> UpdateAsync(InventoryMovements entity)
        {
            try
            {
                if (entity.Id == Guid.Empty)
                {
                    return Response.Error(InvalidInventoryMovementNotFoundMessage);
                }
                var repo = unitOfWork.InventoryMovementsRepository;
                var inventoryMovement = await repo.FindOneAsync(x => x.Id == entity.Id);
                if (inventoryMovement == null)
                {
                    return Response.NotFound(NotFoundMessage);
                }
                await repo.UpdateAsync(im => im.Id == inventoryMovement.Id, entity);
                return Response.Success(inventoryMovement.Id, SuccessUpdateMessage);
            }
            catch (Exception ex)
            {
                return Response.ServerError(ex.Message);
            }
            // Other methods (GetByIdAsync, GetAllAsync, UpdateAsync, DeleteAsync) would be implemented similarly.
        }
    }
}
