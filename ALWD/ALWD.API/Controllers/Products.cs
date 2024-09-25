using Microsoft.AspNetCore.Mvc;
using ALWD.Domain.Entities;
using ALWD.API.Services.ProductService;
using ALWD.Domain.Models;

namespace ALWD.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductService _productService;
        public ProductsController(IProductService productService)
            => _productService = productService;


        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            ResponseData<Product> response;
            try
            {
                response = await _productService.GetProductByIdAsync(id);
			}
            catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}

			if (!response.Successfull)
				return BadRequest(response.ErrorMessage);

			if (response.Data == null)
                return NotFound();

			return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<Product>> GetProductsList([FromQuery] int? itemsPerPage, [FromQuery] string? category, [FromQuery] int? page)
        {
            ResponseData<ListModel<Product>> response;
            try
            {
                response = await _productService.GetProductsAsync(itemsPerPage, category, page);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}

			if (!response.Successfull)
				return BadRequest(response.ErrorMessage);

			if (response.Data == null)
				return NotFound();

			return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromForm] Product product, [FromForm] IFormFile? formFile)
        {
            ResponseData<Product> response;

            try
            {
                response = await _productService.CreateProductAsync(product, formFile);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}

            if (!response.Successfull)
                return BadRequest(response.ErrorMessage);

            //return CreatedAtAction(nameof(GetProduct), new { id = response.Data.Id }, response.Data);
            return Ok();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Product product, IFormFile? formFile)
        {
            ResponseData<Product> response;
			try
			{
				response = await _productService.UpdateProductAsync(product, formFile);
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
        public async Task<IActionResult> DeleteProduct(int id)
        {
			ResponseData<Product> response;
            try
            {
                response = await _productService.DeleteProductAsync(id);
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

