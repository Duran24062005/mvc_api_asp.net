using MovieApi.Modules.Identity.Domain;

namespace MovieApi.Modules.Identity.Application;

public interface IJwtTokenService
{
    TokenResult CreateToken(UserAccount user);
}
