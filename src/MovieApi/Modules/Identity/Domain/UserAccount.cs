namespace MovieApi.Modules.Identity.Domain;

public sealed record UserAccount(
    Guid Id,
    string Username,
    string DisplayName,
    string Email,
    string Role,
    string PasswordHash);
