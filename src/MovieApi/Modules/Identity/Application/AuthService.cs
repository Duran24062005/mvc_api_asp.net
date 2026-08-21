using MovieApi.Modules.Identity.Contracts.Auth;
using MovieApi.Modules.Identity.Mapping;

namespace MovieApi.Modules.Identity.Application;

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
