using MovieApi.Contracts.Users;

namespace MovieApi.Contracts.Auth;

public sealed record LoginResponse(
    string AccessToken,
    DateTimeOffset ExpiresAtUtc,
    UserResponse User);
