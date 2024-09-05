using Microsoft.AspNetCore.Mvc;

namespace ALWD.UI.Components
{
	public class TotalCartPrice : ViewComponent
	{
		public IViewComponentResult Invoke()
		{
			return View();
		}
	}
}
