using AlmaDeMalta.api.Requests;
using AlmaDeMalta.api.Responses;
using AlmaDeMalta.Common.Contracts.Contracts;
using System.Linq.Expressions;

namespace AlmaDeMalta.Api.Services;
    public interface IBaseService<T>
    {
    // Create
    Task<Response> CreateAsync(T entity);
    // Read
    Task<Response> GetByIdAsync(Guid id);
    Task<Response> GetAllAsync();
    // Update
    Task<Response> UpdateAsync(T entity);
    // Delete
    Task<Response> DeleteAsync(Guid id);
    // Search
    Task<Response> Search(Expression<Func<T, bool>> searchTerm);
}
