using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ALWD.Domain.Entities;
using ALWD.UI.Services.CategoryService;

namespace ALWD.UI.Admin.Pages.CategoryPages
{
    public class EditModel : PageModel
    {

        [BindProperty]
        public Category Category { get; set; } = default!;
        private readonly ICategoryService _categoryService;

        public EditModel(ICategoryService service)
        {
            _categoryService = service;
        }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response =  await _categoryService.GetCategoryByIdAsync(id.Value);
            if (response.Data == null)
            {
                return NotFound();
            }
            Category = response.Data;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _categoryService.UpdateCategoryAsync(Category);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            return RedirectToPage("./Index");
        }

        private async Task<bool> CategoryExists(int id)
        {
            var response = await _categoryService.GetCategoryByIdAsync(id);
            return response.Successfull;
        }
    }
}
