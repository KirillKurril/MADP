using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.AspNetCore.Http;

public class AllowedExtensions : ValidationAttribute
{
    private readonly string[] _extensions;

    public AllowedExtensions(string[] extensions, string errorMessage = null)
        : base(errorMessage)
    {
        _extensions = extensions;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            if (!_extensions.Contains(extension.ToLower()))
            {
                return new ValidationResult(ErrorMessage ?? $"File extension not allowed. Allowed extensions: {string.Join(", ", _extensions)}");
            }
        }
		else if (value is string fileName)
		{
			var extension = Path.GetExtension(fileName);
			if (!_extensions.Contains(extension.ToLower()))
			{
				return new ValidationResult(ErrorMessage ?? $"File extension not allowed. Allowed extensions: {string.Join(", ", _extensions)}");
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
