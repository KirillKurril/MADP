using ALWD.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ALWD.UI.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			ViewData["PageTitle"] = "Лабораторная работа 2";
			ViewData["CurrentSection"] = "MainMenu";
			var items = new List<ListDemo>
			{
				new ListDemo { Id = 1, Name = "Item 1" },
				new ListDemo { Id = 2, Name = "Item 2" },
				new ListDemo { Id = 3, Name = "Item 3" }
			};

			SelectList Items = new SelectList(items, "Id", "Name");
			ViewData["Items"] = Items;
			return View();
		}
	}
}