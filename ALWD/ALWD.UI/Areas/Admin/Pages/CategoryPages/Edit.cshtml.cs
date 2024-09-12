using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ALWD.API.Data;
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(Category.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
