using AlmaDeMalta.api.Requests;
using AlmaDeMalta.api.Responses;
using AlmaDeMalta.Common.Contracts.Contracts;
using FastEndpoints;

namespace AlmaDeMalta.api.Mappers;
public class UserMapper: Mapper<UserRequest, Response, User>
{
    public override User ToEntity(UserRequest source)
    {
        return new User
        {
            Id = source.Id,
            Name = source.Name,
            Email = source.Email,
            AuthProviderId = source.AuthProviderId,
            Phone = source.Phone,
            Role = source.Role
        };
    }
}
