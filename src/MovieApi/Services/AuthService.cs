using MovieApi.Contracts.Auth;
using MovieApi.Mapping;
using MovieApi.Repositories;

namespace MovieApi.Services;

public sealed class AuthService(IUserRepository users, IJwtTokenService tokens) : IAuthService
{
    public LoginResponse? Login(LoginRequest request)
    {
        var user = users.ValidateCredentials(request.Username, request.Password);
        if (user is null)
        {
            return null;
        }

        var token = tokens.CreateToken(user);
        return new LoginResponse(token.AccessToken, token.ExpiresAtUtc, user.ToResponse());
    }
}
