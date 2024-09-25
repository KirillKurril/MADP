using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ALWD.Domain.Entities;
using ALWD.UI.Services.ProductService;
using Microsoft.AspNetCore.Mvc;
using ALWD.Domain.Models;

namespace ALWD.UI.Admin.Pages.ProductPages
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;
		public ListModel<Product> Products { get; set; } = default!;

		public IndexModel(IProductService service)
        {
            _productService = service;
        }
        
        public async Task<IActionResult> OnGetAsync(int pageNo = 1)
        {
            ResponseData<ListModel<Product>> response;

			try
            {
                response = await _productService.GetProductListAsync(null, pageNo);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            Products = response.Data;

            return Page();
        }
    }
}
