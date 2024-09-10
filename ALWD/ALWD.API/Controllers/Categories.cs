using ALWD.API.Services.CategoryService;
using ALWD.Domain.Entities;
using ALWD.Domain.Models;
using Microsoft.AspNetCore.Mvc;


namespace ALWD.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ApiCategoriesController : ControllerBase
	{
		private ICategoryService _categoryService;
		public ApiCategoriesController(ICategoryService categoryService)
			=> _categoryService = categoryService;

		[HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategoryById(int id)
        {
            ResponseData<Category> response = await _categoryService.GetCategorytByIdAsync(id);

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


    }
}

