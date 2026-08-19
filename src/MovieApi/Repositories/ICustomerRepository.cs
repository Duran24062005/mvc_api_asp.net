using MovieApi.Domain;

namespace MovieApi.Repositories;

public interface ICustomerRepository
{
    IReadOnlyCollection<Customer> GetAll();
    Customer? GetById(Guid id);
    Customer Add(string fullName, string email, string? phoneNumber);
    bool Update(Guid id, string fullName, string email, string? phoneNumber);
    bool Delete(Guid id);
}
