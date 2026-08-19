using MovieApi.Authentication;
using MovieApi.Domain;

namespace MovieApi.Services;

public interface IJwtTokenService
{
    TokenResult CreateToken(UserAccount user);
}
