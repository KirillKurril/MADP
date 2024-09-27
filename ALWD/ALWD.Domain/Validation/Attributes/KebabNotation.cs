using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class KebabNotation : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is string strValue)
        {
            var regex = new Regex(@"^[a-zA-Z][a-zA-Z0-9-]*[a-zA-Z]$");

            if (!regex.IsMatch(strValue))
            {
                return new ValidationResult(ErrorMessage ?? "The string must be in kebab-notation format:" +
                                            "start and end with a letter, contain only Latin letters and the '-' character.");
            }
        }

        return ValidationResult.Success;
    }
}
