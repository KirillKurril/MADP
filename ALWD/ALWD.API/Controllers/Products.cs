using Microsoft.AspNetCore.Mvc;
using ALWD.Domain.Entities;
using ALWD.API.Services.ProductService;
using ALWD.Domain.Models;
using ALWD.Domain.DTOs;

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
                return StatusCode(500, response.ErrorMessage);

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
                return StatusCode(500, response.ErrorMessage);

            if (response.Data == null)
				return NotFound();

			return Ok(response);
        }

        /*  [HttpPost]
          [Consumes("multipart/form-data")]
          public async Task<ActionResult<Product>> CreateProduct([FromForm] Product product, [FromForm] IFormFile? file)
          {
              if (file == null || file.Length == 0)
              {
                  return BadRequest("File not provided.");
              }

              // Ваш сервис для создания продукта
              ResponseData<Product> response;
              try
              {
                  response = await _productService.CreateProductAsync(product, file);
              }
              catch (Exception ex)
              {
                  return StatusCode(500, ex.Message);
              }

              if (!response.Successfull)
                  return BadRequest(response.ErrorMessage);

              return Ok(response.Data);
          }*/

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(CreateProductDTO dto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            Product product = new Product()
            {
                Name = dto.ProductName,
                Description = dto.ProductDescription,
                Price = dto.ProductPrice,
                Quantity = dto.ProductQuantity,
                CategoryId = dto.ProductCategoryId,
            };

            IFormFile image = new ALWD.Domain.DTOs.FormFile(
                new MemoryStream(dto.ImageContent),
                "productImage",
                dto.ImageName,
                dto.ImageMimeType);
            
            ResponseData<Product> response;
            try
            {
                response = await _productService.CreateProductAsync(product, image);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            if (!response.Successfull)
                return StatusCode(500, response.ErrorMessage);

            return Ok(response.Data.Id);
        }


        [HttpPut]
        public async Task<IActionResult> UpdateProduct(UpdateProductDTO dto)
        {
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

            ResponseData<Product> prevProductResponse;
            try
            {
                prevProductResponse = await _productService.GetProductByIdAsync(dto.ProductId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            if (!prevProductResponse.Successfull)
                return StatusCode(500, prevProductResponse.ErrorMessage);

            if (prevProductResponse.Data == null)
                return NotFound();

            Product product = new Product()
            {
                Id = dto.ProductId,
                Name = dto.ProductName,
                Description = dto.ProductDescription,
                Price = dto.ProductPrice,
                Quantity = dto.ProductQuantity,
                CategoryId = dto.ProductCategoryId,
                Image = prevProductResponse.Data.Image
            };

            IFormFile? image;
            if (dto.ImageContent == null) 
                image = null;
            else
                image = new Domain.DTOs.FormFile(new MemoryStream(dto.ImageContent),
                                                "productImage",
                                                dto.ImageName,
                                                dto.ImageMimeType);

            ResponseData<Product> response;
			try
			{
				response = await _productService.UpdateProductAsync(product, image);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}

            if (!response.Successfull)
                return StatusCode(500, response.ErrorMessage);


            return Ok(response.Data.Id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
			ResponseData<bool> response;
            try
            {
                response = await _productService.DeleteProductAsync(id);
            }
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}

            if (!response.Successfull)
                return StatusCode(500, response.ErrorMessage);

            return Ok();
        }
    }

}

