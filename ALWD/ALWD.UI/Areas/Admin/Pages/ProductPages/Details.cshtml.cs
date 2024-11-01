using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ALWD.Domain.Entities;
using ALWD.UI.Services.ProductService;
using ALWD.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace ALWD.UI.Admin.Pages.ProductPages
{
    [Authorize(Policy = "admin")]
    public class DetailsModel : PageModel
    {
        private readonly IProductService _productService;

        public DetailsModel(IProductService service)
        {
            _productService = service;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
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
                Product = product.Data;
            }
            return Page();
        }
    }
}
