using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ALWD.Domain.Entities;
using ALWD.UI.Services.ProductService;
using ALWD.UI.Services.CategoryService;
using ALWD.Domain.Validation.Models;
using ALWD.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace ALWD.UI.Admin.Pages.ProductPages
{
    [Authorize(Policy = "admin")]
    public class EditModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        [BindProperty]
        public ProductEditValidationModel Model { get; set; }

        public EditModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Model = new ProductEditValidationModel();
            if (id == null || id <= 0)
            {
                return NotFound();
            }

            ResponseData<Product> product;
            try
            {
                product = await _productService.GetProductByIdAsync(id.Value);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            if (product == null)
            {
                return NotFound($"Product with id {id} can't be found");
            }
            else
            {
                Model.Name = product.Data.Name;
                Model.Description = product.Data.Description;
                Model.Price = product.Data.Price;
                Model.Quantity = product.Data.Quantity;
                Model.Id = product.Data.Id; 
            }

            var categoryList = await _categoryService.GetCategoryListAsync();
            ViewData["CategoryId"] = new SelectList(categoryList.Data, "Id", "Name", product.Data.CategoryId);
            ViewData["PrevImageURL"] = product.Data.Image.URL;
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var categoryList = await _categoryService.GetCategoryListAsync();
                ViewData["CategoryId"] = new SelectList(categoryList.Data, "Id", "Name");
                return Page();
            }

            ResponseData<int> updatedIdResponse;
            try
            {
                updatedIdResponse = await _productService.UpdateProductAsync(Model);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            if (!updatedIdResponse.Successfull)
                return NotFound(updatedIdResponse.ErrorMessage);

            return RedirectToPage("./Details", new { id = updatedIdResponse.Data });
        }

        private async Task<bool> ProductExists(int id)
        {
            var response = await _productService.GetProductByIdAsync(id);
            return response.Data != null;
        }
    }
}
