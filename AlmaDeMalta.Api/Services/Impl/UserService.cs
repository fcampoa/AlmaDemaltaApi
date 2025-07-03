using AlmaDeMalta.api.Responses;
using AlmaDeMalta.Common.Contracts.Contracts;
using AlmaDeMalta.Common.Contracts.DataBase;
using System.Linq.Expressions;

namespace AlmaDeMalta.api.Services.Impl;
public class UserService(IUnitOfWork unitOfWork, ILogger<UserService> _logger) : IUserService
{
    private readonly string SuccessCreateMessage = "User created successfully.";
    private readonly string SuccessGetAllMessage = "Users retrieved successfully.";
    private readonly string SuccessGetByIdMessage = "User retrieved successfully.";
    private readonly string SuccessUpdateMessage = "User updated successfully.";
    private readonly string SuccessDeleteMessage = "User deleted successfully.";
    private readonly string NotFoundMessage = "User not found.";
    private readonly string InvalidUserNotFoundMessage = "User not found with the given ID.";
    private readonly string InvalidUserDeleteMessage = "Invalid user ID for deletion.";
    private readonly string InvalidAuthUserNotFoundMessage = "User not found with the given ID.";
    private readonly string SuccessGetByAuthIdMessage = "User retrieved successfully by AuthProviderId.";
    public async Task<Response> CreateAsync(User entity)
    {
        entity.Id = Guid.NewGuid();
        await unitOfWork.GetRepository<User>().CreateAsync(entity);
        _logger.LogInformation($"User created with ID: {entity.Id}");
        return Response.Success(entity, SuccessCreateMessage, System.Net.HttpStatusCode.Created);
    }

    public async Task<Response> DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            return Response.Error(InvalidUserDeleteMessage);
        }
        var repo = unitOfWork.GetRepository<User>();
        var user = await repo.FindOneAsync(x => x.Id == id);
        if (user == null)
        {
            _logger.LogWarning(NotFoundMessage);
            return Response.NotFound(NotFoundMessage);
        }
        await repo.DeleteAsync(x => x.Id == id);
        _logger.LogInformation($"User deleted with ID: {id}");
        return Response.Success(id, SuccessDeleteMessage);
    }

    public async Task<Response> GetAllAsync()
    {
        var users = await unitOfWork.GetRepository<User>().GetAsync(u => u.ItemType.Contains(nameof(User)));
        if (users == null || !users.Any())
        {
            _logger.LogWarning(NotFoundMessage);
            return Response.NotFound(NotFoundMessage);
        }
        _logger.LogInformation($"Retrieved {users.Count} users from the database.");
        return Response.Success(users, SuccessGetAllMessage);
    }

    public async Task<Response> GetByAuthId(User user)
    {
        var repo = unitOfWork.GetRepository<User>();
        var existingUser = await repo.FindOneAsync(u => u.AuthProviderId == user.AuthProviderId);
        if (existingUser == null)
        {
            await repo.CreateAsync(user);
            _logger.LogInformation($"New user created with AuthProviderId: {user.AuthProviderId}");
            return Response.Success(user, SuccessCreateMessage, System.Net.HttpStatusCode.OK);
        }
        if (string.IsNullOrEmpty(existingUser.AuthProviderId))
        {
            existingUser.AuthProviderId = user.AuthProviderId;
            await repo.UpdateAsync(u => u.Id == existingUser.Id, existingUser);
        }
        _logger.LogInformation($"User retrieved with AuthProviderId: {user.AuthProviderId}");
        return Response.Success(existingUser, SuccessGetByAuthIdMessage);
    }

    public async Task<Response> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            return Response.Error(InvalidUserNotFoundMessage);
        }
        var user = await unitOfWork.GetRepository<User>().FindOneAsync(u => u.Id == id);
        if (user == null)
        {
            _logger.LogWarning(NotFoundMessage);
            return Response.NotFound(NotFoundMessage);
        }
        _logger.LogInformation($"User retrieved with ID: {id}");
        return Response.Success(user, SuccessGetByIdMessage);
    }

    public async Task<Response> Search(Expression<Func<User, bool>> searchTerm)
    {
        var users = await unitOfWork.GetRepository<User>().GetAsync(searchTerm);
        if (users == null || !users.Any())
        {
            _logger.LogWarning(NotFoundMessage);
            return Response.NotFound(NotFoundMessage);
        }
        _logger.LogInformation($"Found {users.Count} users matching the search criteria.");
        return Response.Success(users, SuccessGetAllMessage);
    }

    public async Task<Response> UpdateAsync(User entity)
    {
        if (entity == null)
        {
            _logger.LogWarning("User update failed: Entity cannot be null.");
            return Response.Error("User entity cannot be null.");
        }
        if (entity.Id == Guid.Empty)
        {
            _logger.LogWarning(InvalidUserNotFoundMessage);
            return Response.Error(InvalidUserNotFoundMessage);
        }
        var existingUser = await unitOfWork.GetRepository<User>().ExistsAsync(u => u.Id == entity.Id);
        if (!existingUser)
        {
            _logger.LogWarning(NotFoundMessage);
            return Response.NotFound(NotFoundMessage);
        }
        await unitOfWork.GetRepository<User>().UpdateAsync(u => u.Id == entity.Id, entity);
        _logger.LogInformation($"User updated with ID: {entity.Id}");
        return Response.Success(entity, SuccessUpdateMessage);
    }
}
