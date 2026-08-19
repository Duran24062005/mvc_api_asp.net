namespace MovieApi.Contracts.Customers;

public sealed record CustomerResponse(
    Guid Id,
    string FullName,
    string Email,
    string? PhoneNumber,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset UpdatedAtUtc);
