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
		private readonly ICategoryService _categoryService;
		private readonly IProductService _productService;

		public CatalogController(ICategoryService categoryService, IProductService productService)
			=> (_categoryService, _productService) = (categoryService, productService);

		public async Task<IActionResult> Index(string? category, int page = 1)
		{

			var categoriesResponse = await _categoryService.GetCategoryListAsync();

			if (categoriesResponse == null || !categoriesResponse.Successfull)
			{
				return NotFound(categoriesResponse == null ? "Category list is unavailable" : categoriesResponse.ErrorMessage);
			}

			Category selectedCategory = new();

			if (string.IsNullOrEmpty(category))
			{
				ViewData["currentCategory"] = "Все";
			}
			else
			{
				try
				{
					selectedCategory = JsonConvert.DeserializeObject<Category>(category) ?? new Category();
				}
				catch (JsonException)
				{
					return BadRequest("Invalid category format.");
				}
				if(categoriesResponse.Data.Contains(selectedCategory))
					ViewData["currentCategory"] = selectedCategory.Name;
				else if(categoriesResponse.Data.Any(c => c.Id == selectedCategory.Id && c.Name == selectedCategory.Name && c.NormalizedName == selectedCategory.NormalizedName))
					ViewData["currentCategory"] = selectedCategory.Name;
				else
					ViewData["currentCategory"] = "Все";
			}

			ViewData["currentCategoryNormilizedName"] = selectedCategory.NormalizedName;


			var productResponse = await _productService.GetProductListAsync(selectedCategory.NormalizedName, page);
			if (productResponse == null || !productResponse.Successfull)
			{
				return NotFound(productResponse == null ? "Product list is unavailable" : productResponse.ErrorMessage);
			}

			if (Request.IsAjaxRequest())
			{
				return PartialView("_ListPartial", productResponse.Data);
			}

			var viewModel = new CatalogViewModel(productResponse, categoriesResponse);
			return View(viewModel);
		}
	}
}
