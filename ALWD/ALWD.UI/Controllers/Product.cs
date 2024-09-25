using ALWD.UI.Services.ProductService;
using ALWD.Domain.Entities;
using ALWD.Domain.Models;
using ALWD.UI.Services.CategoryService;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ALWD.UI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }
    }
}
