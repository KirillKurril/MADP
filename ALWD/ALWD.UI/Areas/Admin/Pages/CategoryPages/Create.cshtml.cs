using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ALWD.Domain.Entities;
using ALWD.UI.Services.CategoryService;

namespace ALWD.UI.Admin.Pages.CategoryPages
{
    public class CreateModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public CreateModel(ICategoryService service)
        {
            _categoryService = service;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
             await _categoryService.CreateCategoryAsync(Category);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return RedirectToPage("./Index");
        }
    }
}
