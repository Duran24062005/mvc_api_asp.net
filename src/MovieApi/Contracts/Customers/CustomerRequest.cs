using System.ComponentModel.DataAnnotations;

namespace MovieApi.Contracts.Customers;

public sealed class CustomerRequest
{
    [Required]
    [StringLength(160, MinimumLength = 2)]
    public string FullName { get; init; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(320)]
    public string Email { get; init; } = string.Empty;

    [StringLength(40)]
    public string? PhoneNumber { get; init; }
}
