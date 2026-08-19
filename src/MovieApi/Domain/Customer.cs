namespace MovieApi.Domain;

public sealed record Customer(
    Guid Id,
    string FullName,
    string Email,
    string? PhoneNumber,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset UpdatedAtUtc);
