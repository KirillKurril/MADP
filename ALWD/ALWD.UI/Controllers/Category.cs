using Microsoft.AspNetCore.Mvc;

namespace ALWD.UI.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ShowCreatePage()
        {
            return View("Create");
        }
        public IActionResult ShowUpdatePage()
        {
            return View("Update");
        }
        public IActionResult ShowDeletePage()
        {
            return View("Delete");
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Update()
        {
            return View();
        }
        public IActionResult Delete()
        {
            return View();
        }

    }
}
