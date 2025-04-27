using AlmaDeMalta.api.Mappers;
using AlmaDeMalta.api.Requests;
using AlmaDeMalta.api.Responses;
using AlmaDeMalta.Common.Contracts.DataBase;
using System.Net;

namespace AlmaDeMalta.Api.Services;
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
                           .WithStatus(HttpStatusCode.Created) // Usar HttpStatusCode en lugar de int
                           .WithSuccessMessage(SuccessCreateMessage)
                           .Build();
        }
        catch (Exception)
        {
            throw; // Mantener el throw para propagar la excepción
        }
    }

    public async Task<Response> DeleteAsync(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
            {
                return Response.CreateBuilder()
                               .AsError(InvalidProductDeleteMessage, HttpStatusCode.BadRequest) // Usar HttpStatusCode
                               .Build();
            }
            var repo = unitOfWork.ProductRepository;
            var product = await repo.FindOneAsync(x => x.Id == id);
            if (product == null)
            {
                return Response.CreateBuilder()
                               .AsError(InvalidProductNotFoundMessage, HttpStatusCode.NotFound) // Usar HttpStatusCode
                               .Build();
            }
            await repo.DeleteAsync(x => x.Id == id);
            return Response.CreateBuilder()
                           .WithBody(id)
                           .WithStatus(HttpStatusCode.OK) // Usar HttpStatusCode
                           .WithSuccessMessage(SuccessDeleteMessage)
                           .Build();
        }
        catch (Exception)
        {
            throw; // Mantener el throw para propagar la excepción
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
                          .WithStatus(products.Any() ?HttpStatusCode.OK : HttpStatusCode.NotFound) // Usar HttpStatusCode
                          .WithSuccessMessage(SuccessGetAllMessage)
                          .Build();
        }
        catch (Exception)
        {
            throw; // Mantener el throw para propagar la excepción
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
                return Response.CreateBuilder()
                               .AsError(NotFoundMessage, HttpStatusCode.NotFound) // Usar HttpStatusCode
                               .Build();
            }
            return Response.CreateBuilder()
                           .WithBody(product)
                           .WithStatus(HttpStatusCode.OK) // Usar HttpStatusCode
                           .WithSuccessMessage(SuccessGetByIdMessage)
                           .Build();
        }
        catch (Exception)
        {
            throw; // Mantener el throw para propagar la excepción
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
                return Response.CreateBuilder()
                               .AsError(InvalidProductNotFoundMessage, HttpStatusCode.NotFound) // Usar HttpStatusCode
                               .Build();
            }
            var mapper = new ProductMapper();
            var product = mapper.ToEntity(request);
            product.Id = request.Id;
            await repo.UpdateAsync(x => x.Id == product.Id, product);
            return Response.CreateBuilder()
                           .WithBody(product.Id)
                           .WithStatus(HttpStatusCode.OK) // Usar HttpStatusCode
                           .WithSuccessMessage(SuccessUpdateMessage)
                           .Build();
        }
        catch (Exception)
        {
            throw; // Mantener el throw para propagar la excepción
        }
    }
}
