using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ALWD.Domain.Entities;
using ALWD.UI.Services.CategoryService;
using Microsoft.AspNetCore.Authorization;

namespace ALWD.UI.Admin.Pages.CategoryPages
{
    [Authorize(Policy = "admin")]
    public class IndexModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        public IReadOnlyList<Category> CategoryList { get; set; } = default!;

        public IndexModel(ICategoryService service)
        {
            _categoryService = service;
        }

        public async Task OnGetAsync()
        {
            var response = await _categoryService.GetCategoryListAsync();
            CategoryList = response.Data;
        }
    }
}
