using AlmaDeMalta.api.Responses;
using AlmaDeMalta.api.Services.Impl;
using AlmaDeMalta.Api.Services;
using AlmaDeMalta.Common.Contracts.Attributes;
using AlmaDeMalta.Common.Contracts.Contracts;

namespace AlmaDeMalta.api.Services;
[ServiceClass(typeof(UserService), StrategyEnum.Scoped)]
public interface IUserService : IBaseService<User>
{
    Task<Response> GetByAuthId(User user);
}
