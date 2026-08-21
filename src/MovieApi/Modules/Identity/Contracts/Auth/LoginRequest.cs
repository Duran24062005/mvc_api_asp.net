using System.ComponentModel.DataAnnotations;

namespace MovieApi.Modules.Identity.Contracts.Auth;

public sealed class LoginRequest
{
    [Required]
    [StringLength(80, MinimumLength = 3)]
    public string Username { get; init; } = string.Empty;

    [Required]
    [StringLength(120, MinimumLength = 8)]
    public string Password { get; init; } = string.Empty;
}
