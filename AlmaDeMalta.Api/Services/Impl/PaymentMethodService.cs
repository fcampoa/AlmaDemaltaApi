using AlmaDeMalta.api.Responses;
using AlmaDeMalta.Common.Contracts.Contracts;
using AlmaDeMalta.Common.Contracts.DataBase;
using System.Linq.Expressions;

namespace AlmaDeMalta.api.Services.Impl;
public class PaymentMethodService(IUnitOfWork unitOfWork, ILogger<PaymentMethodService> _logger) : IPaymentMethodService
{
    private readonly string SuccessCreateMessage = "Payment method created successfully.";
    private readonly string SuccessGetAllMessage = "Payment methods retrieved successfully.";
    private readonly string SuccessGetByIdMessage = "Payment method retrieved successfully.";
    private readonly string SuccessUpdateMessage = "Payment method updated successfully.";
    private readonly string SuccessDeleteMessage = "Payment method deleted successfully.";
    private readonly string NotFoundMessage = "Payment method not found.";
    private readonly string InvalidPaymentMethodNotFoundMessage = "Payment method not found with the given ID.";
    private readonly string InvalidPaymentMethodDeleteMessage = "Invalid payment method ID for deletion.";

    public async Task<Response> CreateAsync(PaymentMethod entity)
    {
        entity.Id = Guid.NewGuid();
        await unitOfWork.GetRepository<PaymentMethod>().CreateAsync(entity);
        _logger.LogInformation($"Payment method created with ID: {entity.Id}");
        return Response.Success(entity, SuccessCreateMessage, System.Net.HttpStatusCode.Created);
    }

    public async Task<Response> DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            return Response.Error(InvalidPaymentMethodDeleteMessage);
        }
        var repo = unitOfWork.GetRepository<PaymentMethod>();
        var paymentMethod = await repo.FindOneAsync(x => x.Id == id);
        if (paymentMethod == null)
        {
            return Response.NotFound(NotFoundMessage);
        }
        await repo.DeleteAsync(x => x.Id == id);
        _logger.LogInformation($"Payment method deleted with ID: {id}");
        return Response.Success(id, SuccessDeleteMessage);
    }

    public async Task<Response> GetAllAsync()
    {
        var paymentMethods = await unitOfWork.GetRepository<PaymentMethod>().GetAsync();
        _logger.LogInformation($"Retrieved {paymentMethods.Count} payment methods from the database.");
        if (paymentMethods == null || !paymentMethods.Any())
        {
            return Response.NotFound(NotFoundMessage);
        }
        return Response.Success(paymentMethods, SuccessGetAllMessage);
    }

    public async Task<Response> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            return Response.Error(InvalidPaymentMethodNotFoundMessage);
        }
        var paymentMethod = await unitOfWork.GetRepository<PaymentMethod>().FindOneAsync(x => x.Id == id);
        if (paymentMethod == null)
        {
            return Response.NotFound(NotFoundMessage);
        }
        _logger.LogInformation($"Payment method retrieved with ID: {id}");
        return Response.Success(paymentMethod, SuccessGetByIdMessage);
    }

    public async Task<Response> Search(Expression<Func<PaymentMethod, bool>> searchTerm)
    {
        var paymentMethods = await unitOfWork.GetRepository<PaymentMethod>().GetAsync(searchTerm);
        if (paymentMethods == null || !paymentMethods.Any())
        {
            return Response.NotFound(NotFoundMessage);
        }
        _logger.LogInformation($"Found {paymentMethods.Count} payment methods matching the search criteria.");
        return Response.Success(paymentMethods, "Payment methods found successfully.");
    }

    public async Task<Response> UpdateAsync(PaymentMethod entity)
    {     
        var repo = unitOfWork.GetRepository<PaymentMethod>();
        var existingPaymentMethod = await repo.FindOneAsync(x => x.Id == entity.Id);
        if (existingPaymentMethod == null)
        {
            return Response.NotFound(InvalidPaymentMethodNotFoundMessage);
        }
        await repo.UpdateAsync(x => x.Id == entity.Id, entity);
        _logger.LogInformation($"Payment method updated with ID: {entity.Id}");
        return Response.Success(entity, SuccessUpdateMessage);
    }
}
