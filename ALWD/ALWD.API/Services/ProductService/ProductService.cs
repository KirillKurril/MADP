using ALWD.UI.Services.CategoryService;
using ALWD.Domain.Entities;
using ALWD.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace ADLW1.Services.ProductService
{
	public class ProductService : IProductService
	{
        private List<Product> _products = new();
        private List<Category> _categories = new();
		private IConfiguration _config;
        public ProductService(
									[FromServices] IConfiguration config,
							        ICategoryService categoryService)
        {
            _categories = categoryService.GetCategoryListAsync()
		   .Result
		   .Data;
            _config = config;
        }


        public int PageNo { get; }

        public Task<ResponseData<Product>> CreateProductAsync(Product product, IFormFile? formFile)
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
	}
}
