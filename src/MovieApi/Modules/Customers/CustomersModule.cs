using Microsoft.Extensions.DependencyInjection;
using MovieApi.Modules.Customers.Application;
using MovieApi.Modules.Customers.Infrastructure;

namespace MovieApi.Modules.Customers;

public static class CustomersModule
{
    public static IServiceCollection AddCustomersModule(this IServiceCollection services)
    {
        services.AddSingleton<ICustomerRepository, InMemoryCustomerRepository>();

        return services;
    }
}
