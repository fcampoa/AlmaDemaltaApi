using AlmaDeMalta.api.Responses;
using AlmaDeMalta.Common.Contracts.Contracts;
using AlmaDeMalta.Common.Contracts.DataBase;
using AlmaDeMalta.Common.Contracts.Overviews;
using System.Linq.Expressions;

namespace AlmaDeMalta.api.Services.Impl;
public class SaleDashboardService(IUnitOfWork unitOfWork, ILogger<SaleDashboardService> _logger) : ISaleDashboardService
{
    private readonly string SuccessCreateMessage = "Sale dashboard created successfully.";
    private readonly string SuccessGetAllMessage = "Sale dashboards retrieved successfully.";
    private readonly string SuccessGetByIdMessage = "Sale dashboard retrieved successfully.";
    private readonly string SuccessUpdateMessage = "Sale dashboard updated successfully.";
    private readonly string SuccessDeleteMessage = "Sale dashboard deleted successfully.";
    private readonly string NotFoundMessage = "Sale dashboard not found.";
    private readonly string InvalidDashboardNotFoundMessage = "Sale dashboard not found with the given ID.";
    private readonly string InvalidDashboardDeleteMessage = "Invalid sale dashboard ID for deletion.";

    public async Task<Response> CreateAsync(SaleDashboard entity)
    {
        entity.Id = Guid.NewGuid();
        if (entity.IsDefault)
        {
            await unitOfWork.GetRepository<SaleDashboard>().UpdateManyAsync(x => x.IsDefault, x => new SaleDashboard { IsDefault = false});
        }
        await unitOfWork.GetRepository<SaleDashboard>().CreateAsync(entity);
        _logger.LogInformation($"Sale dashboard created with ID: {entity.Id}");
        return Response.Success(entity, SuccessCreateMessage, System.Net.HttpStatusCode.Created);
    }

    public async Task<Response> DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            return Response.Error(InvalidDashboardDeleteMessage);
        }
        var repo = unitOfWork.GetRepository<SaleDashboard>();
        var dashboard = await repo.FindOneAsync(x => x.Id == id);
        if (dashboard == null)
        {
            return Response.NotFound(NotFoundMessage);
        }
        await repo.DeleteAsync(x => x.Id == id);
        _logger.LogInformation($"Dashboard deleted with ID: {id}");
        return Response.Success(id, SuccessDeleteMessage);
    }

    public async Task<Response> FindOne(Expression<Func<SaleDashboard, bool>> searchTerm)
    {
        if (searchTerm == null)
        {
            return Response.Error("Search term cannot be null.");
        }
        var dashboard = await unitOfWork.GetRepository<SaleDashboard>().FindOneAsync(searchTerm);
        if (dashboard == null)
        {
            return Response.NotFound(NotFoundMessage);
        }
        _logger.LogInformation($"Dashboard found with search criteria: {searchTerm}");
        return Response.Success(dashboard, "Sale dashboard found successfully.");
    }

    public async Task<Response> GetAllAsync()
    {
        var dashboards = await unitOfWork.GetRepository<SaleDashboard>().GetAsync(d => d.ItemType.Contains(nameof(SaleDashboard)));
        if (dashboards == null || !dashboards.Any())
        {
            return Response.NotFound(NotFoundMessage);
        }
        _logger.LogInformation($"Retrieved {dashboards.Count} dashboards from the database.");
        return Response.Success(dashboards.Select(d => d.ToSaleDashboardOverview()), SuccessGetAllMessage);
    }

    public async Task<Response> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            return Response.Error(InvalidDashboardNotFoundMessage);
        }
        var dashboard = await unitOfWork.GetRepository<SaleDashboard>().FindOneAsync(x => x.Id == id);
        if (dashboard == null)
        {
            return Response.NotFound(NotFoundMessage);
        }
        _logger.LogInformation($"Dashboard retrieved with ID: {id}");
        return Response.Success(dashboard, SuccessGetByIdMessage);
    }

    public async Task<Response> GetDashboardsOverviewAsync()
    {
        var dashboards = await unitOfWork.GetRepository<SaleDashboard>().GetAsync(d => d.ItemType.Contains(nameof(SaleDashboard)));
        if (dashboards == null || !dashboards.Any())
        {
            return Response.NotFound(NotFoundMessage);
        }
        _logger.LogInformation($"Retrieved {dashboards.Count} dashboards for overview.");
        return Response.Success(dashboards.Select(d => new { d.Id, d.Name, TotalProducts = d.Products.Count }).ToList(), SuccessGetAllMessage);
    }

    public async Task<Response> Search(Expression<Func<SaleDashboard, bool>> searchTerm)
    {
        if (searchTerm == null)
        {
            return Response.Error("Search term cannot be null.");
        }
        var dashboards = await unitOfWork.GetRepository<SaleDashboard>().GetAsync(searchTerm);
        if (dashboards == null || !dashboards.Any())
        {
            return Response.NotFound(NotFoundMessage);
        }
        _logger.LogInformation($"Found {dashboards.Count} dashboards matching the search criteria.");
        return Response.Success(dashboards, SuccessGetAllMessage);
    }

    public async Task<Response> UpdateAsync(SaleDashboard entity)
    {
        if (entity.Id == Guid.Empty)
        {
            return Response.Error(InvalidDashboardNotFoundMessage);
        }
        var existingDashboard = await unitOfWork.GetRepository<SaleDashboard>().FindOneAsync(x => x.Id == entity.Id);
        if (existingDashboard == null)
        {
            return Response.NotFound(NotFoundMessage);
        }
        if (entity.IsDefault)
        {
            await unitOfWork.GetRepository<SaleDashboard>().UpdateManyAsync(x => x.IsDefault, x => new SaleDashboard { IsDefault = false });
        }
        await unitOfWork.GetRepository<SaleDashboard>().UpdateAsync(x => x.Id == entity.Id, entity);
        _logger.LogInformation($"Dashboard updated with ID: {entity.Id}");
        return Response.Success(entity, SuccessUpdateMessage);
    }
}
