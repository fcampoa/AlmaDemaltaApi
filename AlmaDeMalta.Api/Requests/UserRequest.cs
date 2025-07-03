using AlmaDeMalta.Common.Contracts.Contracts;

namespace AlmaDeMalta.api.Requests;
public class UserRequest
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? AuthProviderId { get; set; } = string.Empty;
    public string Phone { get; set; } = null!;
    public RoleEnum Role { get; set; } = RoleEnum.User;
}
