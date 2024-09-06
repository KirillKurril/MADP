using Microsoft.AspNetCore.Mvc;

namespace ALWD.UI.Controllers
{
	public class Cart : Controller
	{
		public IActionResult Index()
		{
			ViewData["CurrentSection"] = "Cart";
			return View();
		}
	}
}
