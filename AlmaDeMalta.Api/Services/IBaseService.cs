using AlmaDeMalta.api.Requests;
using AlmaDeMalta.api.Responses;

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
}
