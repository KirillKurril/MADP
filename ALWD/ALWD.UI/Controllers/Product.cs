using ADLW1.Services.ProductService;
using ALWD.Domain.Entities;
using ALWD.Domain.Models;
using ALWD.UI.Services.CategoryService;
using Microsoft.AspNetCore.Mvc;

namespace ALWD.UI.Controllers
{
	public class ProductController : Controller
	{
        private ICategoryService _categoryService;
        private IProductService _productService;
        public ProductController(ICategoryService categoryService, IProductService productService)
            => (_categoryService, _productService) = (categoryService, productService);
        public async Task<IActionResult> Index()
        {
            var randomCategory = await _categoryService.GetRandomCategory();
            ResponseData<ListModel<Product>> productResponse = await _productService.GetProductListAsync(randomCategory.NormalizedName);
            if (!productResponse.Successfull)
                return NotFound(productResponse.ErrorMessage);
            return View(productResponse.Data.Items);
        }

    }
}
