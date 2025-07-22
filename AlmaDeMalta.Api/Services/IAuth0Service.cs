using AlmaDeMalta.api.Services.Impl;
using AlmaDeMalta.Common.Contracts.Attributes;

namespace AlmaDeMalta.api.Services;
[ServiceClass(typeof(Auth0Service), StrategyEnum.Scoped)]
public interface IAuth0Service
{
    Task<string> GetManagementTokenAsync();
    Task<List<Auth0UserDto>> SyncUsers();
}
