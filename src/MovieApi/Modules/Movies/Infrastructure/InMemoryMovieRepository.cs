using MovieApi.Modules.Movies.Application;
using MovieApi.Modules.Movies.Domain;

namespace MovieApi.Modules.Movies.Infrastructure;

public sealed class InMemoryMovieRepository : IMovieRepository
{
    private readonly object _syncRoot = new();
    private readonly List<Movie> _movies =
    [
        new(
            Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"),
            "The Matrix",
            "Science Fiction",
            1999,
            "The Wachowskis",
            DateTimeOffset.Parse("2026-01-01T00:00:00Z"),
            DateTimeOffset.Parse("2026-01-01T00:00:00Z")),
        new(
            Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"),
            "Inception",
            "Science Fiction",
            2010,
            "Christopher Nolan",
            DateTimeOffset.Parse("2026-01-02T00:00:00Z"),
            DateTimeOffset.Parse("2026-01-02T00:00:00Z"))
    ];

    public IReadOnlyCollection<Movie> GetAll()
    {
        lock (_syncRoot)
        {
            return _movies.OrderBy(movie => movie.Title).ToArray();
        }
    }

    public Movie? GetById(Guid id)
    {
        lock (_syncRoot)
        {
            return _movies.FirstOrDefault(movie => movie.Id == id);
        }
    }

    public Movie Add(string title, string genre, int releaseYear, string director)
    {
        var now = DateTimeOffset.UtcNow;
        var movie = new Movie(
            Guid.NewGuid(),
            Normalize(title),
            Normalize(genre),
            releaseYear,
            Normalize(director),
            now,
            now);

        lock (_syncRoot)
        {
            _movies.Add(movie);
        }

        return movie;
    }

    public bool Update(Guid id, string title, string genre, int releaseYear, string director)
    {
        lock (_syncRoot)
        {
            var index = _movies.FindIndex(movie => movie.Id == id);
            if (index < 0)
            {
                return false;
            }

            _movies[index] = _movies[index] with
            {
                Title = Normalize(title),
                Genre = Normalize(genre),
                ReleaseYear = releaseYear,
                Director = Normalize(director),
                UpdatedAtUtc = DateTimeOffset.UtcNow
            };

            return true;
        }
    }

    public bool Delete(Guid id)
    {
        lock (_syncRoot)
        {
            var index = _movies.FindIndex(movie => movie.Id == id);
            if (index < 0)
            {
                return false;
            }

            _movies.RemoveAt(index);
            return true;
        }
    }

    private static string Normalize(string value) => value.Trim();
}
