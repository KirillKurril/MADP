using Microsoft.AspNetCore.Mvc;

namespace ALWD.UI.Controllers
{
	public class Identity : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult LogOut()
		{
			return View();
		}

	}
}
