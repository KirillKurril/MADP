using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ALWD.Domain.Entities;
using ALWD.UI.Services.ProductService;
using ALWD.UI.Services.CategoryService;

namespace ALWD.UI.Admin.Pages.ProductPages
{
    public class EditModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public EditModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        [BindProperty]
        public IFormFile? ProductImage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.GetProductByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            Product = product.Data;

            var categoryList = await _categoryService.GetCategoryListAsync();
            ViewData["CategoryId"] = new SelectList(categoryList.Data, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Product.Image.MimeType = ProductImage.ContentType;
            try
            {
                await _productService.UpdateProductAsync(Product, ProductImage);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            return RedirectToPage("./Index");
        }

        private async Task<bool> ProductExists(int id)
        {
            var response = await _productService.GetProductByIdAsync(id);
            return response.Data != null;
        }
    }
}
