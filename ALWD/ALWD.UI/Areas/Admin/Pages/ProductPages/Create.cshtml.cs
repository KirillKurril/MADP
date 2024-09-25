using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ALWD.Domain.Entities;
using ALWD.UI.Services.ProductService;
using ALWD.UI.Services.CategoryService;

namespace ALWD.UI.Admin.Pages.ProductPages
{
    public class CreateModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        [BindProperty]
        public Product Product { get; set; } = default!;

        [BindProperty]
        public IFormFile? ProductImage { get; set; }

        public CreateModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> OnGet()
        {
            var categoryList = await _categoryService.GetCategoryListAsync();
            ViewData["CategoryId"] = new SelectList(categoryList.Data, "Id", "Name");
            return Page();
        }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await _productService.CreateProductAsync(Product, ProductImage);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            return RedirectToPage("./Index");
        }
    }
}
