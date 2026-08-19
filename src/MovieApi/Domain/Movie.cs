namespace MovieApi.Domain;

public sealed record Movie(
    Guid Id,
    string Title,
    string Genre,
    int ReleaseYear,
    string Director,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset UpdatedAtUtc);
