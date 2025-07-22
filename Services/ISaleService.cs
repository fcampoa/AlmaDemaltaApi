using AlmaDeMalta.api.Responses;
using AlmaDeMalta.api.Services.Impl;
using AlmaDeMalta.Api.Services;
using AlmaDeMalta.Common.Contracts.Attributes;
using AlmaDeMalta.Common.Contracts.Contracts;

namespace AlmaDeMalta.api.Services;
[ServiceClass(targetClass: typeof(SaleService), strategy: StrategyEnum.Scoped)]
public interface ISaleService : IBaseService<Sale>
{
}
