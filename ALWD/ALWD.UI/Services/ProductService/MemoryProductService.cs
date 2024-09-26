using ALWD.UI.Services.CategoryService;
using ALWD.Domain.Entities;
using ALWD.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using ALWD.Domain.Validation.Models;

namespace ALWD.UI.Services.ProductService
{
	public class MemoryProductService : IProductService
	{
        private List<Product> _products = new();
        private IReadOnlyList<Category> _categories;
		ICategoryService _categoryService;
		private IConfiguration _config;
        public MemoryProductService(
									[FromServices] IConfiguration config,
							        ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _config = config;
            SetupData();

        }


        public int PageNo { get; }

        public Task<ResponseData<Product>> CreateProductAsync(Product product, IFormFile? formFile)
		{
			throw new NotImplementedException();
		}

        public Task CreateProductAsync(ProductValidationModel model)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<ResponseData<Product>> GetProductByIdAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<ResponseData<ListModel<Product>>> GetProductListAsync(string? categoryNormalizedName, int pageNo)
		{
			List<Product> products = new();

            if (string.IsNullOrEmpty(categoryNormalizedName)) 
			{
				products = _products;
			}
			else
			{
                try
                {
                    products = _products.Where(p => p.Category.NormalizedName == categoryNormalizedName).ToList();
                }
                catch (Exception ex)
                {
                    var failResponseData = new ResponseData<ListModel<Product>>(
                        new ListModel<Product>(), false, "Product list receiving fault");
                    return Task.FromResult(failResponseData);
                }
            }
            int itemsPerPage;
            try
            {
                itemsPerPage = int.Parse(_config["ItemsPerPage"]);
            }
            catch
            {
                throw new Exception("Unable to receive items per page configuration");
            }
            int totalPages = (int)Math.Ceiling((double)products.Count / itemsPerPage);

            var productsOnPage = products
                .Skip((pageNo - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToList();


            var listmodel = new ListModel<Product>(productsOnPage);
            var responseData = new ResponseData<ListModel<Product>>(listmodel);
			responseData.Data.CurrentPage = pageNo;
			responseData.Data.TotalPages = totalPages;
            return Task.FromResult(responseData);

        }

		public Task UpdateProductAsync(int id, Product product, IFormFile? formFile)
		{
			throw new NotImplementedException();
		}

        public Task UpdateProductAsync(ProductValidationModel model)
        {
            throw new NotImplementedException();
        }

        Task IProductService.CreateProductAsync(Product product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        Task IProductService.DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<ResponseData<Product>> IProductService.GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<ResponseData<ListModel<Product>>> IProductService.GetProductListAsync(string? categoryNormalizedName, int pageNo)
        {
            throw new NotImplementedException();
        }

        private async void SetupData()
			{
				var categoriesResponse = await _categoryService.GetCategoryListAsync();
				_categories = categoriesResponse.Data;
				_products = new List<Product>
			{
			};

			}

        Task IProductService.UpdateProductAsync(Product product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}
