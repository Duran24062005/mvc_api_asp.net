using MovieApi.Contracts.Customers;
using MovieApi.Contracts.Movies;
using MovieApi.Contracts.Users;
using MovieApi.Domain;

namespace MovieApi.Mapping;

public static class ResponseMappings
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

    public static CustomerResponse ToResponse(this Customer customer)
    {
        return new CustomerResponse(
            customer.Id,
            customer.FullName,
            customer.Email,
            customer.PhoneNumber,
            customer.CreatedAtUtc,
            customer.UpdatedAtUtc);
    }

    public static UserResponse ToResponse(this UserAccount user)
    {
        return new UserResponse(
            user.Id,
            user.Username,
            user.DisplayName,
            user.Email,
            user.Role);
    }
}
