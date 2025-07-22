using AlmaDeMalta.api.Mappers;
using AlmaDeMalta.api.Requests;
using AlmaDeMalta.api.Responses;
using AlmaDeMalta.Common.Contracts.Contracts;
using AlmaDeMalta.Common.Contracts.DataBase;
using System.Linq.Expressions;
using System.Net;

namespace AlmaDeMalta.api.Services.Impl;
    public class InventoryMovementsService(IUnitOfWork unitOfWork, ConversionService conversionService) : IInventoryMovementsService
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
            var productsRepo = unitOfWork.GetRepository<Product>();
            var product = await productsRepo.FindOneAsync(x => x.Id == entity.Product.ProductId);
            if (product == null)
            {
                return Response.NotFound(InvalidInventoryMovementNotFoundMessage);
            }
            entity.Id = Guid.NewGuid();
            await unitOfWork.GetRepository<InventoryMovements>().CreateAsync(entity);

            var stock = conversionService.Convert(entity.Quantity, entity.Unit, product.Unit);
            product.Stock += entity.IsIncoming ? stock : -stock;

            await productsRepo.UpdateAsync(x => x.Id == product.Id, product);

            return Response.Success(entity, SuccessCreateMessage, HttpStatusCode.Created);
        }

        public async Task<Response> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return Response.Error(InvalidInventoryMovementDeleteMessage);
            }
            var repo = unitOfWork.GetRepository<Product>();
            var inventoryMovement = await repo.FindOneAsync(x => x.Id == id);
            if (inventoryMovement == null)
            {
                return Response.NotFound(NotFoundMessage);
            }
            await repo.DeleteAsync(x => x.Id == id);
            return Response.Success(id, SuccessDeleteMessage);
        }

    public async Task<Response> FindOne(Expression<Func<InventoryMovements, bool>> searchTerm)
    {
        if (searchTerm == null)
        {
            return Response.Error("Search term cannot be null.");
        }
        var repo = unitOfWork.GetRepository<InventoryMovements>();
        var inventoryMovement = await repo.FindOneAsync(searchTerm);
        if (inventoryMovement == null)
        {
            return Response.NotFound(NotFoundMessage);
        }
        return Response.Success(inventoryMovement, SuccessGetByIdMessage);
    }

    public async Task<Response> GetAllAsync()
        {
            var repo = unitOfWork.GetRepository<Product>();
            var inventoryMovements = await repo.GetAsync();
            if (inventoryMovements == null || !inventoryMovements.Any())
            {
                return Response.NotFound(NotFoundMessage);
            }
            var response = inventoryMovements;
            return Response.Success(response, SuccessGetAllMessage, HttpStatusCode.OK);
        }

        public async Task<Response> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return Response.Error(InvalidInventoryMovementNotFoundMessage);
            }
            var repo = unitOfWork.GetRepository<Product>();
            var inventoryMovement = await repo.FindOneAsync(x => x.Id == id);
            if (inventoryMovement == null)
            {
                return Response.NotFound(NotFoundMessage);
            }
            var response = inventoryMovement;
            return Response.Success(response, SuccessGetByIdMessage);
        }

        public async Task<Response> Search(Expression<Func<InventoryMovements, bool>> searchTerm)
        {
            if (searchTerm == null)
            {
                return Response.Error("Search term cannot be null.");
            }
            var repo = unitOfWork.GetRepository<InventoryMovements>();
            var inventoryMovements = await repo.GetAsync(searchTerm);
            if (inventoryMovements == null || !inventoryMovements.Any())
            {
                return Response.NotFound(NotFoundMessage);
            }
            return Response.Success(inventoryMovements, SuccessGetAllMessage, HttpStatusCode.OK);
        }

        public async Task<Response> UpdateAsync(InventoryMovements entity)
        {
            if (entity.Id == Guid.Empty)
            {
                return Response.Error(InvalidInventoryMovementNotFoundMessage);
            }
            var repo = unitOfWork.GetRepository<InventoryMovements>();
            var inventoryMovement = await repo.FindOneAsync(x => x.Id == entity.Id);
            if (inventoryMovement == null)
            {
                return Response.NotFound(NotFoundMessage);
            }
            await repo.UpdateAsync(im => im.Id == inventoryMovement.Id, entity);
            return Response.Success(inventoryMovement.Id, SuccessUpdateMessage);
        }
    }
