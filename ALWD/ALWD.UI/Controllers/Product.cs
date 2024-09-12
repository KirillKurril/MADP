using ALWD.UI.Services.ProductService;
using ALWD.Domain.Entities;
using ALWD.Domain.Models;
using ALWD.UI.Services.CategoryService;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ALWD.UI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> ShowCreatePage()
        {
            var categoryResponse = await _categoryService.GetCategoryListAsync();
            IReadOnlyList<Category> categories = categoryResponse.Data;
            ViewBag.CategoryList = new SelectList(categories, "Id", "Name");
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

        [HttpPost]
        public async Task<ActionResult> Create(Product product, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                await _productService.CreateProductAsync(product, image);

                return RedirectToAction("Details", product);
            }

            return View(product);
        }
    }
}
