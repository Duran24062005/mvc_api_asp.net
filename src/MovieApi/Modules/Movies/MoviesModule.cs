using Microsoft.Extensions.DependencyInjection;
using MovieApi.Modules.Movies.Application;
using MovieApi.Modules.Movies.Infrastructure;

namespace MovieApi.Modules.Movies;

public static class MoviesModule
{
    public static IServiceCollection AddMoviesModule(this IServiceCollection services)
    {
        services.AddSingleton<IMovieRepository, InMemoryMovieRepository>();

        return services;
    }
}
