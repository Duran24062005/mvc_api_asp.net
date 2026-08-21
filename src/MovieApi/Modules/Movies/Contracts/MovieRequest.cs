using System.ComponentModel.DataAnnotations;

namespace MovieApi.Modules.Movies.Contracts;

public sealed class MovieRequest : IValidatableObject
{
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Title { get; init; } = string.Empty;

    [Required]
    [StringLength(80, MinimumLength = 2)]
    public string Genre { get; init; } = string.Empty;

    [Range(1888, 9999)]
    public int ReleaseYear { get; init; }

    [Required]
    [StringLength(160, MinimumLength = 2)]
    public string Director { get; init; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var maxYear = DateTime.UtcNow.Year + 1;
        if (ReleaseYear > maxYear)
        {
            yield return new ValidationResult(
                $"Release year cannot be greater than {maxYear}.",
                [nameof(ReleaseYear)]);
        }
    }
}
