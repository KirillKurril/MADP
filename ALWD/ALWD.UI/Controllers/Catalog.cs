using ALWD.UI.Services.ProductService;
using ALWD.Domain.Entities;
using ALWD.Domain.Models;
using ALWD.UI.Services.CategoryService;
using ALWD.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ALWD.UI.Extensions;

namespace ALWD.UI.Controllers
{
	public class CatalogController : Controller
	{
		private ICategoryService _categoryService;
		private IProductService _productService;
		public CatalogController(ICategoryService categoryService, IProductService productService)
			=> (_categoryService, _productService) = (categoryService, productService);
        public async Task<IActionResult> Index(string? category, int page = 1)
        {
			Category selectedCategory = new();
			if(string.IsNullOrEmpty(category))
                ViewData["currentCategory"] = "Все";
			
			else
			{
                selectedCategory = JsonConvert.DeserializeObject<Category>(category);
                ViewData["currentCategory"] = selectedCategory.Name;
            }
            ViewData["currentCategoryNormilizedName"] = selectedCategory.NormalizedName;
            var categories = await _categoryService.GetCategoryListAsync();

			ResponseData<ListModel<Product>> productResponse = await _productService.GetProductListAsync(selectedCategory.NormalizedName, page);
			if (!productResponse.Successfull)
				return NotFound(productResponse.ErrorMessage);

            if (Request.IsAjaxRequest())
                return PartialView("_ListPartial", page); 

            var viewModel = new CatalogViewModel(productResponse, categories);
			return View(viewModel);
		}
	}
}
