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
        if (value is IFormFile fileAsFileForm)
        {
            if (fileAsFileForm.Length > _maxFileSize)
            {
                return new ValidationResult(ErrorMessage ?? $"Maximum allowed file size is {_maxFileSize} bytes.");
            }
        }
		else if (value is byte[] fileAsBytes)
		{
			if (fileAsBytes.LongLength > _maxFileSize)
			{
				return new ValidationResult(ErrorMessage ?? $"Maximum allowed file size is {_maxFileSize} bytes.");
			}
		}
		else
		{
			return new ValidationResult("Invalid attribute type.");
		}

		return ValidationResult.Success;
    }
}
