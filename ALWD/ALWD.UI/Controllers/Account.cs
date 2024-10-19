using ALWD.UI.Services.Authentication;
using ALWD.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ALWD.UI.Controllers
{
	public class AccountController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Register()
		{
			return View("Create", new RegisterUserViewModel());
		}

		[HttpPost]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Create(RegisterUserViewModel user,
		[FromServices] IAuthService authService)
		{
			if (ModelState.IsValid)
			{
				if (user == null)
				{
					return BadRequest();
				}

				var response = await authService.RegisterUserAsync(
					user.Email,
					user.Password,
					user.Name,
					user.Avatar);

				if (response.Successfull)
				{
					return Redirect(Url.Action("Index", "Home"));
				}
				else return BadRequest(response.ErrorMessage);
			}
			return View(user);
		}
	}
}

