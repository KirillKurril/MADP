﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ActionResult<Product>> GetProductByCategoryAndPage([FromQuery] string? category, [FromQuery] int? page)
        {
            ResponseData<ListModel<Product>> response;
            if (page != null && category != null)
                 response = await _productService.GetProductListAsync(category, page.Value);
            
            else if (page != null && category == null) 
                response = await _productService.GetProductListAsync(page.Value);
            
            else if (page == null && category != null)
                response = await _productService.GetProductListAsync(category);

            else
                response = await _productService.GetProductListAsync();

            if (response.Data == null)
                return NotFound();

            if(!response.Successfull)
                return BadRequest(response.ErrorMessage);

            return Ok(response);
        }

    }
}