using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ALWD.Domain.ViewModels
{
	public class RegisterUserViewModel
	{
		[Required]
		public string Email { get; set; }

		public string Name { get; set; }

		[Required]
		public string Password { get; set; }

		[Required]
		[Compare(nameof(Password))]
		public string ConfirmPassword { get; set; }
		public IFormFile? Avatar { get; set; }
	}
}
