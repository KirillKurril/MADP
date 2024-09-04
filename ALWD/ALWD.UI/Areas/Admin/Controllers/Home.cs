using Microsoft.AspNetCore.Mvc;

namespace ALWD.UI.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class Home : Controller
	{
		public IActionResult Index()
		{
			ViewData["CurrentSection"] = "AdminMenu";
			return View();
		}
	}
}
