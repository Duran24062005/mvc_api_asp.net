using MovieApi.Domain;

namespace MovieApi.Repositories;

public interface IUserRepository
{
    IReadOnlyCollection<UserAccount> GetAll();
    UserAccount? GetById(Guid id);
    UserAccount? GetByUsername(string username);
    UserAccount? ValidateCredentials(string username, string password);
}
