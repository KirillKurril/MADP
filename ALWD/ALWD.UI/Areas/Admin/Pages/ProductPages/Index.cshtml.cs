using Microsoft.AspNetCore.Mvc.RazorPages;
using ALWD.Domain.Entities;
using ALWD.UI.Services.ProductService;
using ALWD.UI.Services.CategoryService;
using Microsoft.AspNetCore.Mvc;
using ALWD.Domain.Models;
using ALWD.UI.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace ALWD.UI.Admin.Pages.ProductPages
{
    [Authorize(Policy = "admin")]
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        public ListModel<Product> Products { get; set; } = default!;
        public IReadOnlyList<Category> Categories { get; set; }
        public Category? CurrentCategory { get; set; } = null;
        public int CurrentPage { get; set; } = 1;

        public IndexModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }
        
        public async Task<IActionResult> OnGetAsync([FromQuery] string? category = "", [FromQuery] int page = 1)
        {
            ResponseData<IReadOnlyList<Category>> categoryListResponse;
            try
            {
                categoryListResponse = await _categoryService.GetCategoryListAsync();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            if (!categoryListResponse.Successfull)
                return StatusCode(404, categoryListResponse.ErrorMessage);

            Categories = categoryListResponse.Data;

            CurrentCategory = Categories.FirstOrDefault(c => c.NormalizedName == category, null);
            CurrentPage = page;

            ResponseData<ListModel<Product>> productListResponse;

			try
            {
                productListResponse = await _productService.GetProductListAsync(CurrentCategory == null
                    ? ""
                    : category, page);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            if (!productListResponse.Successfull)
                return StatusCode(404, productListResponse.ErrorMessage);

            Products = productListResponse.Data;

            if (Request.IsAjaxRequest())
                return Partial("IndexPartial", this);

            return Page();
        }
    }
}
