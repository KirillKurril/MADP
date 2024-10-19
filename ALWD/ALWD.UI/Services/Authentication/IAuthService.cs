using ALWD.Domain.Models;

namespace ALWD.UI.Services.Authentication
{
	public interface IAuthService
	{
		Task<ResponseData<bool>> RegisterUserAsync(string email,
		string password,
		string? name,
		IFormFile? avatar);
	}

}
