using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ALWD.Domain.Entities;
using ALWD.UI.Services.CategoryService;
using Microsoft.AspNetCore.Authorization;

namespace ALWD.UI.Admin.Pages.CategoryPages
{
    [Authorize(Policy = "admin")]
    public class DeleteModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public DeleteModel(ICategoryService service)
        {
            _categoryService = service;
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            try
            {
                await _categoryService.DeleteCategoryAsync(id.Value);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            return RedirectToPage("./Index");
        }
    }
}
