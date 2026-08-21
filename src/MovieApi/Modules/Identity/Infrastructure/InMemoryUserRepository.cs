using MovieApi.Modules.Identity.Application;
using MovieApi.Modules.Identity.Domain;
using MovieApi.Modules.Identity.Infrastructure.Security;
using MovieApi.SharedKernel.Authorization;

namespace MovieApi.Modules.Identity.Infrastructure;

public sealed class InMemoryUserRepository : IUserRepository
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly List<UserAccount> _users;

    public InMemoryUserRepository(IPasswordHasher passwordHasher)
    {
        _passwordHasher = passwordHasher;
        _users =
        [
            new(
                Guid.Parse("cccccccc-cccc-cccc-cccc-ccccccccccc1"),
                "admin",
                "Learning Admin",
                "admin@example.com",
                UserRoles.Admin,
                _passwordHasher.HashPassword("Admin123!")),
            new(
                Guid.Parse("cccccccc-cccc-cccc-cccc-ccccccccccc2"),
                "user",
                "Learning User",
                "user@example.com",
                UserRoles.User,
                _passwordHasher.HashPassword("User123!"))
        ];
    }

    public IReadOnlyCollection<UserAccount> GetAll()
    {
        return _users.OrderBy(user => user.Username).ToArray();
    }

    public UserAccount? GetById(Guid id)
    {
        return _users.FirstOrDefault(user => user.Id == id);
    }

    public UserAccount? GetByUsername(string username)
    {
        var normalizedUsername = NormalizeUsername(username);
        return _users.FirstOrDefault(user => user.Username.Equals(normalizedUsername, StringComparison.OrdinalIgnoreCase));
    }

    public UserAccount? ValidateCredentials(string username, string password)
    {
        var user = GetByUsername(username);
        if (user is null)
        {
            return null;
        }

        return _passwordHasher.VerifyPassword(password, user.PasswordHash) ? user : null;
    }

    private static string NormalizeUsername(string username) => username.Trim().ToLowerInvariant();
}
