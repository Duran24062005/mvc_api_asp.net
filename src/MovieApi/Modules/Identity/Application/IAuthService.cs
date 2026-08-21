using MovieApi.Modules.Identity.Contracts.Auth;

namespace MovieApi.Modules.Identity.Application;

public interface IAuthService
{
    LoginResponse? Login(LoginRequest request);
}
