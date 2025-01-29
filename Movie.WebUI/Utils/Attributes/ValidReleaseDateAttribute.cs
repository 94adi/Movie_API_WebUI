namespace Movie.WebUI.Utils.Attributes;

public class ValidReleaseDateAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is DateOnly releaseDate)
        {
            if (releaseDate > DateOnly.FromDateTime(DateTime.Now))
            {
                return new ValidationResult(
                    $"Please enter a release date before {DateOnly.FromDateTime(DateTime.Now.AddDays(1))}");
            }
        }
        return ValidationResult.Success;
    }
}
