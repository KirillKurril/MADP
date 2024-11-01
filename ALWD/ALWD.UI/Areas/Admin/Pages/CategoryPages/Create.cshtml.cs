using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ALWD.Domain.Entities;
using ALWD.UI.Services.CategoryService;
using ALWD.Domain.Validation.Models;
using ALWD.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace ALWD.UI.Admin.Pages.CategoryPages
{
    [Authorize(Policy = "admin")]
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
        public CategoryCreateValidationModel Model { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ResponseData<int> createdIdResponse;
            try
            {
                createdIdResponse = await _categoryService.CreateCategoryAsync(Model);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            if (!createdIdResponse.Successfull)
                return NotFound(createdIdResponse.ErrorMessage);

            return RedirectToPage("./Details", new { id = createdIdResponse.Data });
        }
    }
}
