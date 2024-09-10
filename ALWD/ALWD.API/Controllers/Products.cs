using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ALWD.Domain.Entities;
using ALWD.API.Services.ProductService;
using ALWD.Domain.Models;

namespace ALWD.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiProductsController : ControllerBase
    {
        private IProductService _productService;

        public ApiProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            ResponseData<Product> response = await _productService.GetProductByIdAsync(id);

            if (response.Data == null)
                return NotFound();

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<Product>> GetProductsList([FromQuery] int? itemsPerPage, [FromQuery] string? category, [FromQuery] int? page)
        {
            ResponseData<ListModel<Product>> response;
            if (itemsPerPage != null)
            {
                if (page != null && category != null)
                    response = await _productService.GetProductListAsync(itemsPerPage.Value, category, page.Value);

                else if (page != null && category == null)
                    response = await _productService.GetProductListAsync(itemsPerPage.Value, page.Value);

                else if (page == null && category != null)
                    response = await _productService.GetProductListAsync(itemsPerPage.Value, category);

                else
                    response = await _productService.GetProductListAsync();
            }
            else
                response = await _productService.GetProductListAsync();

            if (response.Data == null)
                return NotFound();

            if(!response.Successfull)
                return BadRequest(response.ErrorMessage);

            return Ok(response);
        }
        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="product"></param>
        /// <param name="formFile"></param>
        /// <returns></returns>
        // POST: api/Products
        // Метод для создания продукта с возможностью загрузки файла
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromForm] Product product, [FromForm] IFormFile? formFile)
        {
            var response = await _productService.CreateProductAsync(product, formFile);

            if (!response.Successfull)
            {
                return BadRequest(response.ErrorMessage);
            }

            return CreatedAtAction(nameof(GetProduct), new { id = response.Data.Id }, response.Data);
        }

        // PUT: api/Products/5
        // Метод для обновления продукта с возможностью загрузки файла
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] Product product, [FromForm] IFormFile? formFile)
        {
            var existingProductResponse = await _productService.GetProductByIdAsync(id);

            if (existingProductResponse.Data == null)
            {
                return NotFound();
            }

            await _productService.UpdateProductAsync(id, product, formFile);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var existingProductResponse = await _productService.GetProductByIdAsync(id);

            if (existingProductResponse.Data == null)
            {
                return NotFound();
            }

            await _productService.DeleteProductAsync(id);

            return NoContent();
        }
    }

}

