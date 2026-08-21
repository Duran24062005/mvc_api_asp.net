using Microsoft.Extensions.DependencyInjection;
using MovieApi.Modules.Identity.Application;
using MovieApi.Modules.Identity.Infrastructure;
using MovieApi.Modules.Identity.Infrastructure.Security;

namespace MovieApi.Modules.Identity;

public static class IdentityModule
{
    public static IServiceCollection AddIdentityModule(this IServiceCollection services)
    {
        services.AddSingleton<IPasswordHasher, Pbkdf2PasswordHasher>();
        services.AddSingleton<IUserRepository, InMemoryUserRepository>();
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddSingleton<IAuthService, AuthService>();

        return services;
    }
}
