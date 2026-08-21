using MovieApi.Modules.Movies.Contracts;
using MovieApi.Modules.Movies.Domain;

namespace MovieApi.Modules.Movies.Mapping;

public static class MovieMappings
{
    public static MovieResponse ToResponse(this Movie movie)
    {
        return new MovieResponse(
            movie.Id,
            movie.Title,
            movie.Genre,
            movie.ReleaseYear,
            movie.Director,
            movie.CreatedAtUtc,
            movie.UpdatedAtUtc);
    }
}
