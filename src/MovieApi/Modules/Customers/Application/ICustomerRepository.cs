using MovieApi.Modules.Customers.Domain;

namespace MovieApi.Modules.Customers.Application;

public interface ICustomerRepository
{
    IReadOnlyCollection<Customer> GetAll();
    Customer? GetById(Guid id);
    Customer Add(string fullName, string email, string? phoneNumber);
    bool Update(Guid id, string fullName, string email, string? phoneNumber);
    bool Delete(Guid id);
}
