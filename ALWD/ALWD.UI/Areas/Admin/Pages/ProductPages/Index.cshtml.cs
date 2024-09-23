using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ALWD.Domain.Entities;
using ALWD.UI.Services.ProductService;
using Microsoft.AspNetCore.Mvc;

namespace ALWD.UI.Admin.Pages.ProductPages
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;

        public IndexModel(IProductService service)
        {
            _productService = service;
        }

        public IList<Product> Product { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var response = await _productService.GetProductListAsync();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            return RedirectToPage("./Index");
        }
    }
}
