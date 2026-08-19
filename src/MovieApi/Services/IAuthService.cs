using MovieApi.Contracts.Auth;

namespace MovieApi.Services;

public interface IAuthService
{
    LoginResponse? Login(LoginRequest request);
}
