using AlmaDeMalta.api.Requests;
using AlmaDeMalta.api.Responses;
using AlmaDeMalta.Api.Services;
using AlmaDeMalta.Common.Contracts.Contracts;
using AlmaDeMalta.Common.Contracts.DataBase;
using AlmaDeMalta.Common.Contracts.Overviews;
using System.Linq.Expressions;
using System.Net;

namespace AlmaDeMalta.api.Services.Impl;
public class ProductService(IUnitOfWork unitOfWork, ConversionService conversionService, ILogger<ProductService> _logger) : IProductService
{
    private readonly string SuccessCreateMessage = "Product created successfully.";
    private readonly string SuccessGetAllMessage = "Products retrieved successfully.";
    private readonly string SuccessGetByIdMessage = "Product retrieved successfully.";
    private readonly string SuccessUpdateMessage = "Product updated successfully.";
    private readonly string SuccessDeleteMessage = "Product deleted successfully.";
    private readonly string NotFoundMessage = "Product not found.";
    private readonly string InvalidProductNotFoundMessage = "Product not found with the given ID.";
    private readonly string InvalidProductDeleteMessage = "Invalid product ID for deletion.";

    public async Task<Response> CreateAsync(Product entity)
    {
            entity.Id = Guid.NewGuid();
            await unitOfWork.GetRepository<Product>().CreateAsync(entity);
            _logger.LogInformation($"Product created with ID: {entity.Id}");
            return Response.Success(entity, SuccessCreateMessage, HttpStatusCode.Created);
        
    }

    public async Task<Response> DeleteAsync(Guid id)
    {
            if (id == Guid.Empty)
            {
                return Response.Error(NotFoundMessage);
            }
            var repo = unitOfWork.GetRepository<Product>();
            var product = await repo.FindOneAsync(x => x.Id == id);
            if (product == null)
            {
                return Response.NotFound(InvalidProductDeleteMessage);
            }
            await repo.DeleteAsync(x => x.Id == id);
            return Response.Success(id, SuccessDeleteMessage);
        
    }

    public async Task<Response> GetAllAsync()
    {
            var products = await unitOfWork.GetRepository<Product>().GetAsync();
            _logger.LogInformation($"Retrieved {products.Count} products from the database.");
            return Response.Success(products.Select(p => p.ToProductOverview()), SuccessGetAllMessage);
        
    }

    public async Task<Response> GetByIdAsync(Guid id)
    {
            var product = await unitOfWork.GetRepository<Product>().FindOneAsync(x => x.Id == id);
            if (product == null)
            {
                return Response.NotFound(InvalidProductNotFoundMessage);
            }
            return Response.Success(product, SuccessGetByIdMessage);
    }

    public async Task<Response> Search(Expression<Func<Product, bool>> searchTerm)
    {
            var products = await unitOfWork.GetRepository<Product>().GetAsync(searchTerm);
            if (products is null || !products.Any())
            {
                return Response.NotFound(NotFoundMessage);
            }
            return Response.Success(products.Select(p => p.ToProductOverview()), SuccessGetAllMessage);
    }

    public async Task<Response> UpdateAsync(Product entity)
    {
            var repo = unitOfWork.GetRepository<Product>();
            var exist = await repo.FindOneAsync(x => x.Id == entity.Id);
            if (exist is null)
            {
                return Response.NotFound(InvalidProductNotFoundMessage);
            }
            entity.Stock = conversionService.Convert(entity.Stock, exist.Unit, entity.Unit);
            await repo.UpdateAsync(x => x.Id == entity.Id, entity);
            return Response.Success(entity.Id, SuccessUpdateMessage);
    }
}
