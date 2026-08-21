using MovieApi.Modules.Customers.Contracts;
using MovieApi.Modules.Customers.Domain;

namespace MovieApi.Modules.Customers.Mapping;

public static class CustomerMappings
{
    public static CustomerResponse ToResponse(this Customer customer)
    {
        return new CustomerResponse(
            customer.Id,
            customer.FullName,
            customer.Email,
            customer.PhoneNumber,
            customer.CreatedAtUtc,
            customer.UpdatedAtUtc);
    }
}
