using MovieApi.Modules.Movies.Domain;

namespace MovieApi.Modules.Movies.Application;

public interface IMovieRepository
{
    IReadOnlyCollection<Movie> GetAll();
    Movie? GetById(Guid id);
    Movie Add(string title, string genre, int releaseYear, string director);
    bool Update(Guid id, string title, string genre, int releaseYear, string director);
    bool Delete(Guid id);
}
