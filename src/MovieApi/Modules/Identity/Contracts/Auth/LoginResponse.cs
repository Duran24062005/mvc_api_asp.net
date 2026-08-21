using MovieApi.Modules.Identity.Contracts.Users;

namespace MovieApi.Modules.Identity.Contracts.Auth;

public sealed record LoginResponse(
    string AccessToken,
    DateTimeOffset ExpiresAtUtc,
    UserResponse User);
