using Microsoft.AspNetCore.Mvc;


namespace ALWD.API.Controllers
{
	public class CategoriesController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}

