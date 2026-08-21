using MovieApi.Modules.Identity.Contracts.Users;
using MovieApi.Modules.Identity.Domain;

namespace MovieApi.Modules.Identity.Mapping;

public static class UserMappings
{
    public static UserResponse ToResponse(this UserAccount user)
    {
        return new UserResponse(
            user.Id,
            user.Username,
            user.DisplayName,
            user.Email,
            user.Role);
    }
}
