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
            ResponseData<Category> response;
            try
            {
                response = await _categoryService.GetCategoryByIdAsync(id);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}

			if (response.Data == null)
                return NotFound();

			if (!response.Successfull)
				return BadRequest(response.ErrorMessage);

			return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<Category>> GetCategoriesAsync()
        {
            ResponseData<IReadOnlyList<Category>> response;
            try
            {
                response = await _categoryService.GetCategoryListAsync();
			}
            catch (Exception ex) 
            {
				return StatusCode(500, ex.Message);
			}
            
            if (response.Data == null)
                return NotFound();

			if (!response.Successfull)
				return BadRequest(response.ErrorMessage);

			return Ok(response);
        }


        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(Category category)
        {
            ResponseData<Category> response;
            try
            {
				response = await _categoryService.CreateCategoryAsync(category);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}

			if (!response.Successfull)
                return BadRequest(response.ErrorMessage);

            return CreatedAtAction(nameof(GetCategory), new { id = response.Data.Id }, response);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, Category category)
        {
            ResponseData<Category> response;
            try
            {
				response = await _categoryService.UpdateCategoryAsync(id, category);
			}
			catch (Exception ex)
            {
				return StatusCode(500, ex.Message);
			}

			if (!response.Successfull)
				return BadRequest(response.ErrorMessage);

			return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            ResponseData<Category> response;
            try
            {
				response = await _categoryService.DeleteCategoryAsync(id);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}

            if (!response.Successfull)
                return BadRequest(response.ErrorMessage);

            return Ok();
        }
    }

}

