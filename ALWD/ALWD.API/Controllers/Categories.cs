using ALWD.API.Services.CategoryService;
using ALWD.Domain.Entities;
using ALWD.Domain.Models;
using Microsoft.AspNetCore.Mvc;


namespace ALWD.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoriesController : ControllerBase
	{
		private ICategoryService _categoryService;
		public CategoriesController(ICategoryService categoryService)
			=> _categoryService = categoryService;

		[HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            ResponseData<Category> response = await _categoryService.GetCategoryByIdAsync(id);

            if (response.Data == null)
                return NotFound();

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<Category>> GetCategoriesAsync()
        {
            ResponseData<IReadOnlyList<Category>> response = await _categoryService.GetCategoryListAsync();
            
            if (response.Data == null)
                return NotFound();

            return Ok(response);
        }


        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(Category category)
        {
            var response = await _categoryService.CreateCategoryAsync(category);

            if (!response.Successfull)
            {
                return BadRequest(response.ErrorMessage);
            }

            return CreatedAtAction(nameof(GetCategory), new { id = response.Data.Id }, response);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, Category category)
        {
            var response = await _categoryService.UpdateCategoryAsync(id, category);
            
            if (!response.Successfull)
            {
                return BadRequest(response.ErrorMessage);
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var response = await _categoryService.DeleteCategoryAsync(id);

            if (!response.Successfull)
            {
                return BadRequest(response.ErrorMessage);
            }

            return Ok();
        }
    }

}

