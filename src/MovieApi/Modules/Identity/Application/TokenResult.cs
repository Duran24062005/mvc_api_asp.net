namespace MovieApi.Modules.Identity.Application;

public sealed record TokenResult(string AccessToken, DateTimeOffset ExpiresAtUtc);
