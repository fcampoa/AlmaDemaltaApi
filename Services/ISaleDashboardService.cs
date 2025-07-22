using AlmaDeMalta.api.Responses;
using AlmaDeMalta.api.Services.Impl;
using AlmaDeMalta.Api.Services;
using AlmaDeMalta.Common.Contracts.Attributes;
using AlmaDeMalta.Common.Contracts.Contracts;

namespace AlmaDeMalta.api.Services;
[ServiceClass(typeof(SaleDashboardService), StrategyEnum.Scoped)]
public interface ISaleDashboardService: IBaseService<SaleDashboard>
{

    Task<Response> GetDashboardsOverviewAsync();
}
