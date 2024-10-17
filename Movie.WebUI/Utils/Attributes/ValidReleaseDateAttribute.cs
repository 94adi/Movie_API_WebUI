namespace Movie.WebUI.Utils.Attributes;

public class ValidReleaseDateAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is DateTime releaseDate)
        {
            if (releaseDate > DateTime.Now)
            {
                return new ValidationResult("Please enter a valid release date");
            }
        }
        return ValidationResult.Success;
    }
}
