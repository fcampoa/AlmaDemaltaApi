using AlmaDeMalta.api.Responses;
using AlmaDeMalta.Common.Contracts.Contracts;
using AlmaDeMalta.Common.Contracts.DataBase;
using System.Linq.Expressions;

namespace AlmaDeMalta.api.Services.Impl;
public class PurchaseOrderNumberPrefixService(IUnitOfWork unitOfWork) : IPurchaseOrderNumberPrefixService
{
    private readonly string SuccessCreateMessage = "Purchase Order Number Prefix created successfully.";
    private readonly string SuccessGetAllMessage = "Purchase Order Number Prefixes retrieved successfully.";
    private readonly string SuccessGetByIdMessage = "Purchase Order Number Prefix retrieved successfully.";
    private readonly string SuccessUpdateMessage = "Purchase Order Number Prefix updated successfully.";
    private readonly string SuccessDeleteMessage = "Purchase Order Number Prefix deleted successfully.";
    private readonly string NotFoundMessage = "Purchase Order Number Prefix not found.";
    private readonly string InvalidPurchaseOrderNumberPrefixNotFoundMessage = "Purchase Order Number Prefix not found with the given ID.";
    private readonly string InvalidPurchaseOrderNumberPrefixDeleteMessage = "Invalid Purchase Order Number Prefix ID for deletion.";

    public async Task<Response> CreateAsync(PurchaseOrderNumberPrefix entity)
    {
        if (entity == null)
        {
            return Response.Error("Entity cannot be null.");
        }
        entity.Id = Guid.NewGuid();
        var repo = unitOfWork.GetRepository<PurchaseOrderNumberPrefix>();
        await repo.CreateAsync(entity);
        return Response.Success(entity, SuccessCreateMessage, System.Net.HttpStatusCode.Created);
    }

    public async Task<Response> DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            return Response.Error(InvalidPurchaseOrderNumberPrefixDeleteMessage);
        }
        var repo = unitOfWork.GetRepository<PurchaseOrderNumberPrefix>();
        var prefix = await repo.FindOneAsync(x => x.Id == id);
        if (prefix == null)
        {
            return Response.NotFound(NotFoundMessage);
        }
        await repo.DeleteAsync(x => x.Id == id);
        return Response.Success(id, SuccessDeleteMessage);
    }

    public async Task<Response> FindOne(Expression<Func<PurchaseOrderNumberPrefix, bool>> searchTerm)
    {
        if (searchTerm == null)
        {
            return Response.Error("Search term cannot be null.");
        }
        var prefix = await unitOfWork.GetRepository<PurchaseOrderNumberPrefix>().FindOneAsync(searchTerm);
        if (prefix == null)
        {
            return Response.NotFound(NotFoundMessage);
        }
        return Response.Success(prefix, "Purchase Order Number Prefix found successfully.");
    }

    public async Task<Response> GetAllAsync()
    {
        var prefixes = await unitOfWork.GetRepository<PurchaseOrderNumberPrefix>().GetAsync(p => p.ItemType.Contains(nameof(PurchaseOrderNumberPrefix)));
        if (prefixes == null || !prefixes.Any())
        {
            return Response.NotFound(NotFoundMessage);
        }
        return Response.Success(prefixes, SuccessGetAllMessage);
    }

    public async Task<Response> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            return Response.Error(InvalidPurchaseOrderNumberPrefixNotFoundMessage);
        }
        var prefix = await unitOfWork.GetRepository<PurchaseOrderNumberPrefix>().FindOneAsync(x => x.Id == id);
        if (prefix == null)
        {
            return Response.NotFound(NotFoundMessage);
        }
        return Response.Success(prefix, SuccessGetByIdMessage);
    }

    public async Task<Response> Search(Expression<Func<PurchaseOrderNumberPrefix, bool>> searchTerm)
    {
        if (searchTerm == null)
        {
            return Response.Error("Search term cannot be null.");
        }
        var prefixes = await unitOfWork.GetRepository<PurchaseOrderNumberPrefix>().GetAsync(searchTerm);
        if (prefixes == null || !prefixes.Any())
        {
            return Response.NotFound(NotFoundMessage);
        }
        return Response.Success(prefixes, SuccessGetAllMessage);
    }

    public async Task<Response> UpdateAsync(PurchaseOrderNumberPrefix entity)
    {
        if (entity == null || entity.Id == Guid.Empty)
        {
            return Response.Error("Entity cannot be null and must have a valid ID.");
        }
        var repo = unitOfWork.GetRepository<PurchaseOrderNumberPrefix>();
        var existingPrefix = await repo.FindOneAsync(x => x.Id == entity.Id);
        if (existingPrefix == null)
        {
            return Response.NotFound(NotFoundMessage);
        }
        repo.Update(x => x.Id == entity.Id, entity);
        return Response.Success(existingPrefix, SuccessUpdateMessage);
    }
}
