using ALWD.UI.Services.CategoryService;
using ALWD.Domain.Entities;
using ALWD.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using ALWD.Domain.Abstractions;

namespace ADLW1.Services.ProductService
{
	public class ProductService() : IProductService
	{
        private readonly IRepository<Product> _repository;
        private List<Product> _products = new();
        private List<Category> _categories = new();
		private IConfiguration _config;
        public int PageNo { get; }

        public ProductService(IRepository<Product> repository,
							    [FromServices] IConfiguration config,
							    ICategoryService categoryService) : this()
        {
            _categories = categoryService.GetCategoryListAsync()
		   .Result
		   .Data;
            _repository = repository;
            _config = config;
        }
///////!!!!!!!!!!!!!!!!!! Затребовало инициализацию конструктора this()? зачем


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

		public async Task<ResponseData<ListModel<Product>>> GetProductListAsync(string? categoryNormalizedName, int pageNo)
		{
			IReadOnlyList<Product> products;

            if (string.IsNullOrEmpty(categoryNormalizedName)) 
			{
                products = await _repository.ListAllAsync() ;
			}
			else
			{
                try
                {
                    products = await _repository.ListAsync(p => p.Category.NormalizedName == categoryNormalizedName);
                }
                catch (Exception ex)
                {
                    var failResponseData = new ResponseData<ListModel<Product>>(
                        new ListModel<Product>(), false, "Product list receiving fault");
                    return failResponseData;
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
            return responseData;

        }

		public Task UpdateProductAsync(int id, Product product, IFormFile? formFile)
		{
			throw new NotImplementedException();
		}
	}
}
