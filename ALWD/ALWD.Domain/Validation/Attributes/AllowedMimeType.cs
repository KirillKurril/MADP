using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

public class AllowedMimeType : ValidationAttribute
{
	private readonly string[] _mimeTypes;

	public AllowedMimeType(string[] mimeTypes, string errorMessage = null)
		: base(errorMessage)
	{
		_mimeTypes = mimeTypes;
	}

	protected override ValidationResult IsValid(object value, ValidationContext validationContext)
	{
		if (value is IFormFile formFile)
		{
			var mimeType = formFile.ContentType;
			if (!_mimeTypes.Contains(mimeType.ToLower()))
			{
				return new ValidationResult(ErrorMessage ?? $"MIME-тип файла не поддерживается. Разрешённые типы: {string.Join(", ", _mimeTypes)}");
			}
		}
		else if(value is string mimeType)
		{
			if (!_mimeTypes.Contains(mimeType.ToLower()))
			{
				return new ValidationResult(ErrorMessage ?? $"MIME-тип файла не поддерживается. Разрешённые типы: {string.Join(", ", _mimeTypes)}");
			}
		}
		else
		{
			return new ValidationResult("Invalid attribute type.");
		}

		return ValidationResult.Success;
	}
}
