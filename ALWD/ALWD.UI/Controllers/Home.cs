using Microsoft.AspNetCore.Mvc;

namespace ALWD.UI.Controllers
{
	public class Home : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
