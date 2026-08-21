namespace MovieApi.Modules.Identity.Contracts.Users;

public sealed record UserResponse(
    Guid Id,
    string Username,
    string DisplayName,
    string Email,
    string Role);
