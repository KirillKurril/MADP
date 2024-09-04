using Microsoft.AspNetCore.Mvc;

namespace ALWD.UI.Components
{
	public class TotalCartPrice : ViewComponent
	{
		public IViewComponentResult Invoke()
		{
			double randomNumber = new Random().NextDouble() * 100;
			return View();// randomNumber.ToString("0.00");
		}
	}
}
