namespace MovieApi.Authentication;

public sealed record TokenResult(string AccessToken, DateTimeOffset ExpiresAtUtc);
