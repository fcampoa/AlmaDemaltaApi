using AlmaDeMalta.api.Mappers;
using AlmaDeMalta.api.Requests;
using AlmaDeMalta.api.Responses;
using AlmaDeMalta.Common.Contracts.DataBase;

namespace AlmaDeMalta.Api.Services;
public class ProductService(IAlmaDeMaltaUnitOfWork unitOfWork) : IProductService
{
    private readonly string SuccessCreateMessage = "Product created successfully.";
    private readonly string SuccessGetAllMessage = "Products retrieved successfully.";
    private readonly string SuccessGetByIdMessage = "Product retrieved successfully.";
    private readonly string SuccessUpdateMessage = "Product updated successfully.";
    private readonly string SuccessDeleteMessage = "Product deleted successfully.";
    private readonly string ErrorMessage = "An error occurred while processing your request.";
    private readonly string NotFoundMessage = "Product not found.";
    private readonly string InvalidProductNotFoundMessage = "Product not found with the given ID.";
    private readonly string InvalidProductDeleteMessage = "Invalid product ID for deletion.";
    public async Task<Response> CreateAsync(ProductRequest request)
    {
        try
        {
            var mapper = new ProductMapper();
            var product = mapper.ToEntity(request);
            product.Id = Guid.NewGuid();
            await unitOfWork.ProductRepository.CreateAsync(product);

            return Response.CreateBuilder()
                           .WithBody(product.Id)
                           .WithStatus(201)
                           .WithSuccessMessage(SuccessCreateMessage)
                           .Build();
        }
        catch (Exception ex)
        {
            return Response.CreateBuilder().AsError($"{ErrorMessage}: {ex.Message}", 500).Build();
        }
    }
    public async Task<Response> DeleteAsync(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
            {
                return Response.CreateBuilder().AsError(InvalidProductDeleteMessage, 400).Build();
            }
            var repo = unitOfWork.ProductRepository;
            var product = await repo.FindOneAsync(x => x.Id == id);
            if (product == null)
            {
                return Response.CreateBuilder().AsError(InvalidProductNotFoundMessage, 404).Build();
            }
            await repo.DeleteAsync(x => x.Id == id);
            return Response.CreateBuilder().WithBody(id).WithStatus(200).WithSuccessMessage(SuccessDeleteMessage).Build();
        }
        catch (Exception ex)
        {
            return Response.CreateBuilder().AsError($"{ErrorMessage}: {ex.Message}", 500).Build();
        }
    }
    public async Task<Response> GetAllAsync()
    {
        var builder = Response.CreateBuilder();
        try
        {
            var repo = unitOfWork.ProductRepository;
            var products = await repo.GetAsync();
            return builder.WithBody(products)
                          .AsSuccess(SuccessGetAllMessage)
                          .Build();
        }
        catch (Exception ex)
        {
            return builder.AsError($"{ErrorMessage}: {ex.Message}", 500).Build();
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
                return Response.CreateBuilder().AsError(NotFoundMessage, 404).Build();
            }
            return Response.CreateBuilder()
                           .WithBody(product)
                           .WithStatus(200)
                           .WithSuccessMessage(SuccessGetByIdMessage)
                           .Build();
        }
        catch (Exception ex)
        {
            return Response.CreateBuilder().AsError($"{ErrorMessage}: {ex.Message}", 500).Build();
        }
    }
    public async Task<Response> UpdateAsync(ProductRequest request)
    {
        try
        {
            var repo = unitOfWork.ProductRepository;
            var exist = await repo.ExistsAsync(x => x.Id == request.Id);
            if (!exist)
            {
                return Response.CreateBuilder().AsError(InvalidProductNotFoundMessage, 404).Build();
            }
            var mapper = new ProductMapper();
            var product = mapper.ToEntity(request);
            product.Id = request.Id;
            await repo.UpdateAsync(x => x.Id == product.Id, product);
            return Response.CreateBuilder()
                           .WithBody(product.Id)
                           .WithStatus(200)
                           .WithSuccessMessage(SuccessUpdateMessage)
                           .Build();
        }
        catch (Exception ex)
        {
            return Response.CreateBuilder().AsError($"{ErrorMessage}: {ex.Message}", 500).Build();
        }
    }
}
