using AlmaDeMalta.api.Responses;
using AlmaDeMalta.Common.Contracts.Contracts;
using AlmaDeMalta.Common.Contracts.DataBase;
using System.Linq.Expressions;

namespace AlmaDeMalta.api.Services.Impl;
public class SaleService(IUnitOfWork unitOfWork, ILogger<SaleService> _logger) : ISaleService
{
    private readonly string SuccessCreateMessage = "Sale created successfully.";
    private readonly string SuccessGetAllMessage = "Sales retrieved successfully.";
    private readonly string SuccessGetByIdMessage = "Sale retrieved successfully.";
    private readonly string SuccessUpdateMessage = "Sale updated successfully.";
    private readonly string SuccessDeleteMessage = "Sale deleted successfully.";
    private readonly string NotFoundMessage = "Sale not found.";
    private readonly string InvalidSaleNotFoundMessage = "Sale not found with the given ID.";
    private readonly string InvalidSaleDeleteMessage = "Invalid sale ID for deletion.";

    public async Task<Response> CreateAsync(Sale entity)
    {
        entity.Id = Guid.NewGuid();

        entity.Status = entity.PaymentMethod.Type == PaymentType.Complimentary ? StatusEnum.Pending : StatusEnum.Completed;
        var orderNumber = await unitOfWork.GetRepository<PurchaseOrderNumberPrefix>().FindOneAsync(p => p.Id == entity.PurchaseOrderNumberPrefixId);
        if (orderNumber != null)
        {
            entity.PurchaseOrderNumber = $"{orderNumber.prefix}{orderNumber.Number}";
            orderNumber.Number = orderNumber.Number + 1;
            await unitOfWork.GetRepository<PurchaseOrderNumberPrefix>().UpdateAsync(p => p.Id == orderNumber.Id, orderNumber);
        }
        await unitOfWork.GetRepository<Sale>().CreateAsync(entity);
        _logger.LogInformation($"Sale created with ID: {entity.Id}");
        return Response.Success(entity, SuccessCreateMessage, System.Net.HttpStatusCode.Created);
    }
    public async Task<Response> DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            return Response.Error(InvalidSaleDeleteMessage);
        }
        var repo = unitOfWork.GetRepository<Sale>();
        var sale = await repo.FindOneAsync(x => x.Id == id);
        if (sale == null)
        {
            return Response.NotFound(NotFoundMessage);
        }
        await repo.DeleteAsync(x => x.Id == id);
        _logger.LogInformation($"Sale deleted with ID: {id}");
        return Response.Success(id, SuccessDeleteMessage);
    }
    public async Task<Response> GetAllAsync()
    {
        var sales = await unitOfWork.GetRepository<Sale>().GetAsync(s => s.ItemType.Contains(nameof(Sale)));
        if (sales == null || !sales.Any())
        {
            return Response.NotFound(NotFoundMessage);
        }
        _logger.LogInformation($"Retrieved {sales.Count} sales from the database.");
        return Response.Success(sales, SuccessGetAllMessage);
    }

    public async Task<Response> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            return Response.Error(InvalidSaleNotFoundMessage);
        }
        var sale = await unitOfWork.GetRepository<Sale>().FindOneAsync(x => x.Id == id);
        if (sale == null)
        {
            return Response.NotFound(InvalidSaleNotFoundMessage);
        }
        _logger.LogInformation($"Sale retrieved with ID: {id}");
        return Response.Success(sale, SuccessGetByIdMessage);
    }

    public async Task<Response> Search(Expression<Func<Sale, bool>> searchTerm)
    {
        var sales = await unitOfWork.GetRepository<Sale>().GetAsync(searchTerm);
        if (sales == null || !sales.Any())
        {
            return Response.NotFound(NotFoundMessage);
        }
        _logger.LogInformation($"Found {sales.Count} sales matching the search criteria.");
        return Response.Success(sales, SuccessGetAllMessage);
    }

    public async Task<Response> UpdateAsync(Sale entity)
    {
        if (entity.Id == Guid.Empty)
        {
            return Response.Error(NotFoundMessage);
        }
        var existingSale = await unitOfWork.GetRepository<Sale>().FindOneAsync(x => x.Id == entity.Id);
        if (existingSale == null)
        {
            return Response.NotFound(NotFoundMessage);
        }
        await unitOfWork.GetRepository<Sale>().UpdateAsync(x => x.Id == entity.Id, entity);
        _logger.LogInformation($"Sale updated with ID: {entity.Id}");
        return Response.Success(entity, SuccessUpdateMessage);
    }
}
