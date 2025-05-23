using AlmaDeMalta.api.Mappers;
using AlmaDeMalta.api.Requests;
using AlmaDeMalta.api.Responses;
using AlmaDeMalta.Api.Services;
using AlmaDeMalta.Common.Contracts.Contracts;
using AlmaDeMalta.Common.Contracts.DataBase;
using AlmaDeMalta.Common.Contracts.Overviews;
using System.Net;

namespace AlmaDeMalta.api.Services.Impl;
public class ProductService(IAlmaDeMaltaUnitOfWork unitOfWork) : IProductService
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
        try
        {
            entity.Id = Guid.NewGuid();
            await unitOfWork.ProductRepository.CreateAsync(entity);

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
                return Response.Error(NotFoundMessage);
            }
            var repo = unitOfWork.ProductRepository;
            var product = await repo.FindOneAsync(x => x.Id == id);
            if (product == null)
            {
                return Response.NotFound(InvalidProductDeleteMessage);
            }
            await repo.DeleteAsync(x => x.Id == id);
            return Response.Success(id, SuccessDeleteMessage);
        }
        catch (Exception)
        {
           return  Response.ServerError(NotFoundMessage);
        }
    }

    public async Task<Response> GetAllAsync()
    {
        try
        {
            var repo = unitOfWork.ProductRepository;
            var products = await repo.GetAsync();
            return Response.Success(products.Select(p => p.ToProductOverview()), SuccessGetAllMessage);
        }
        catch (Exception ex)
        {
            return Response.ServerError($"{ex.Message}");
        }
    }

    public async Task<Response> GetByIdAsync(Guid id)
    {
        try
        {
            var repo = unitOfWork.ProductRepository;
            var product = await repo.FindOneAsync(x => x.Id == id);
            if (product == null)
            {
                return Response.NotFound(InvalidProductNotFoundMessage);
            }
            return Response.Success(product, SuccessGetByIdMessage);
        }
        catch (Exception ex)
        {
            return Response.ServerError(ex.Message);
        }
    }

    public async Task<Response> UpdateAsync(Product entity)
    {
        try
        {
            var repo = unitOfWork.ProductRepository;
            var exist = await repo.ExistsAsync(x => x.Id == entity.Id);
            if (!exist)
            {
                return Response.NotFound(InvalidProductNotFoundMessage);
            }
            await repo.UpdateAsync(x => x.Id == entity.Id, entity);
            return Response.Success(entity.Id, SuccessUpdateMessage);
        }
        catch (Exception ex)
        {
            return Response.ServerError(ex.Message);
        }
    }
}
