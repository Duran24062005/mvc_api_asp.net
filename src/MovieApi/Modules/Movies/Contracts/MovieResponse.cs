namespace MovieApi.Modules.Movies.Contracts;

public sealed record MovieResponse(
    Guid Id,
    string Title,
    string Genre,
    int ReleaseYear,
    string Director,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset UpdatedAtUtc);
