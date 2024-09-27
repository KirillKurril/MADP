using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

public class MaxStringLength : ValidationAttribute
{
    private readonly int _maxStringLength;

    public MaxStringLength(int maxStringLength, string errorMessage = null)
        : base(errorMessage)
    {
        _maxStringLength = maxStringLength;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is string str)
        {
            if(!string.IsNullOrEmpty(str))
                if (str.Length > _maxStringLength)
                {
                    return new ValidationResult(ErrorMessage ?? $"Maximum allowed length is {_maxStringLength} characters.");
                }
        }
        else if (value is null)
        {
            return ValidationResult.Success;
        }
        else
        {
            return new ValidationResult("Invalid attribute type.");
        }

        return ValidationResult.Success;
    }
}
