using MovieApi.Modules.Customers.Application;
using MovieApi.Modules.Customers.Domain;

namespace MovieApi.Modules.Customers.Infrastructure;

public sealed class InMemoryCustomerRepository : ICustomerRepository
{
    private readonly object _syncRoot = new();
    private readonly List<Customer> _customers =
    [
        new(
            Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
            "Maria Gomez",
            "maria.gomez@example.com",
            "+57 300 000 0001",
            DateTimeOffset.Parse("2026-01-01T00:00:00Z"),
            DateTimeOffset.Parse("2026-01-01T00:00:00Z")),
        new(
            Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
            "John Carter",
            "john.carter@example.com",
            "+1 555 0100",
            DateTimeOffset.Parse("2026-01-02T00:00:00Z"),
            DateTimeOffset.Parse("2026-01-02T00:00:00Z"))
    ];

    public IReadOnlyCollection<Customer> GetAll()
    {
        lock (_syncRoot)
        {
            return _customers.OrderBy(customer => customer.FullName).ToArray();
        }
    }

    public Customer? GetById(Guid id)
    {
        lock (_syncRoot)
        {
            return _customers.FirstOrDefault(customer => customer.Id == id);
        }
    }

    public Customer Add(string fullName, string email, string? phoneNumber)
    {
        var now = DateTimeOffset.UtcNow;
        var customer = new Customer(
            Guid.NewGuid(),
            NormalizeText(fullName),
            NormalizeEmail(email),
            NormalizeOptionalText(phoneNumber),
            now,
            now);

        lock (_syncRoot)
        {
            _customers.Add(customer);
        }

        return customer;
    }

    public bool Update(Guid id, string fullName, string email, string? phoneNumber)
    {
        lock (_syncRoot)
        {
            var index = _customers.FindIndex(customer => customer.Id == id);
            if (index < 0)
            {
                return false;
            }

            _customers[index] = _customers[index] with
            {
                FullName = NormalizeText(fullName),
                Email = NormalizeEmail(email),
                PhoneNumber = NormalizeOptionalText(phoneNumber),
                UpdatedAtUtc = DateTimeOffset.UtcNow
            };

            return true;
        }
    }

    public bool Delete(Guid id)
    {
        lock (_syncRoot)
        {
            var index = _customers.FindIndex(customer => customer.Id == id);
            if (index < 0)
            {
                return false;
            }

            _customers.RemoveAt(index);
            return true;
        }
    }

    private static string NormalizeText(string value) => value.Trim();

    private static string NormalizeEmail(string value) => value.Trim().ToLowerInvariant();

    private static string? NormalizeOptionalText(string? value)
    {
        var normalized = value?.Trim();
        return string.IsNullOrWhiteSpace(normalized) ? null : normalized;
    }
}
