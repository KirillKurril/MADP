﻿using ADLW1.Services.ProductService;
using ALWD.Domain.Entities;
using ALWD.Domain.Models;
using ALWD.UI.Services.CategoryService;
using ALWD.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ALWD.UI.Controllers
{
	public class CatalogController : Controller
	{
		private ICategoryService _categoryService;
		private IProductService _productService;
		public CatalogController(ICategoryService categoryService, IProductService productService)
			=> (_categoryService, _productService) = (categoryService, productService);
        public async Task<IActionResult> Index(string? category)
        {
			ViewData["CurrentSection"] = "Catalog";
			if(string.IsNullOrEmpty(category))
                ViewData["currentCategory"] = "Все";
			
			else
			{
				var categoryFull = await _categoryService.GetByNormilizedName(category);
                ViewData["currentCategory"] = categoryFull.Name;
            }

			var categories = await _categoryService.GetCategoryListAsync();

			ResponseData<ListModel<Product>> productResponse = await _productService.GetProductListAsync(category);

			if (!productResponse.Successfull)
				return NotFound(productResponse.ErrorMessage);

			var viewModel = new CatalogViewModel(productResponse.Data, categories.Data);
			return View(viewModel);
		}
	}
}