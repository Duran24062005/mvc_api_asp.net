using MovieApi.Modules.Identity.Domain;

namespace MovieApi.Modules.Identity.Application;

public interface IUserRepository
{
    IReadOnlyCollection<UserAccount> GetAll();
    UserAccount? GetById(Guid id);
    UserAccount? GetByUsername(string username);
    UserAccount? ValidateCredentials(string username, string password);
}
