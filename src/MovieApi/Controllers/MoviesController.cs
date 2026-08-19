using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApi.Constants;
using MovieApi.Contracts.Movies;
using MovieApi.Mapping;
using MovieApi.Repositories;

namespace MovieApi.Controllers;

[ApiController]
[Authorize(Roles = UserRoles.AdminOrUser)]
[Route("api/movies")]
public sealed class MoviesController(IMovieRepository movies) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IReadOnlyCollection<MovieResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<IReadOnlyCollection<MovieResponse>> GetAll()
    {
        var response = movies.GetAll().Select(movie => movie.ToResponse()).ToArray();
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<MovieResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<MovieResponse> GetById(Guid id)
    {
        var movie = movies.GetById(id);
        return movie is null ? NotFound() : Ok(movie.ToResponse());
    }

    [HttpPost]
    [ProducesResponseType<MovieResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<MovieResponse> Create(MovieRequest request)
    {
        var movie = movies.Add(request.Title, request.Genre, request.ReleaseYear, request.Director);
        return CreatedAtAction(nameof(GetById), new { id = movie.Id }, movie.ToResponse());
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(Guid id, MovieRequest request)
    {
        return movies.Update(id, request.Title, request.Genre, request.ReleaseYear, request.Director)
            ? NoContent()
            : NotFound();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        return movies.Delete(id) ? NoContent() : NotFound();
    }
}
