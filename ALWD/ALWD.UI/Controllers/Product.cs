using ALWD.UI.Services.ProductService;
using ALWD.Domain.Entities;
using ALWD.Domain.Models;
using ALWD.UI.Services.CategoryService;
using Microsoft.AspNetCore.Mvc;

namespace ALWD.UI.Controllers
{
	public class ProductController : Controller
	{
        public IActionResult Index()
        {
            return View();
        }

    }
}
