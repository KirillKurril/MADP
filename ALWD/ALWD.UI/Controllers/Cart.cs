using Microsoft.AspNetCore.Mvc;

namespace ALWD.UI.Controllers
{
	public class Cart : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
