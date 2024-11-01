using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ALWD.Domain.Entities;
using ALWD.UI.Services.CategoryService;
using ALWD.Domain.Models;
using ALWD.Domain.Validation.Models;
using Microsoft.AspNetCore.Authorization;

namespace ALWD.UI.Admin.Pages.CategoryPages
{
    [Authorize(Policy = "admin")]
    public class EditModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        [BindProperty]
        public CategoryEditValidationModel Model { get; set; }

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

            ResponseData<Category> response;
            try
            {
                response = await _categoryService.GetCategoryByIdAsync(id.Value);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            
            if(!response.Successfull)
                return NotFound(response.ErrorMessage);

            if (response.Data == null)
            {
                return NotFound();
            }
            Model = new CategoryEditValidationModel()
            {
                Id = response.Data.Id,
                Name = response.Data.Name,
                NormalizedName = response.Data.NormalizedName
            };

            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ResponseData<int> updatedIdResponse;
            try
            {
                updatedIdResponse = await _categoryService.UpdateCategoryAsync(Model);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            if (!updatedIdResponse.Successfull)
                return NotFound(updatedIdResponse.ErrorMessage);

            return RedirectToPage("./Details", new { id = updatedIdResponse.Data });
        }

        private async Task<bool> CategoryExists(int id)
        {
            var response = await _categoryService.GetCategoryByIdAsync(id);
            return response.Successfull;
        }
    }
}
