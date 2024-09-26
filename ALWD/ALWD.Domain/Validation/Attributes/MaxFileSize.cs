using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

public class MaxFileSize : ValidationAttribute
{
    private readonly int _maxFileSize;

    public MaxFileSize(int maxFileSize, string errorMessage = null)
        : base(errorMessage)
    {
        _maxFileSize = maxFileSize;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is IFormFile file)
        {
            if (file.Length > _maxFileSize)
            {
                return new ValidationResult(ErrorMessage ?? $"Maximum allowed file size is {_maxFileSize} bytes.");
            }
        }

        return ValidationResult.Success;
    }
}
