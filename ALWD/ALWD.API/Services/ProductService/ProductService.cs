using ALWD.Domain.Entities;
using ALWD.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using ALWD.Domain.Abstractions;


namespace ALWD.API.Services.ProductService
{
	public class ProductService() : IProductService
	{
        private readonly IRepository<Product> _repository;
		private IConfiguration _config;

        public ProductService(IRepository<Product> repository,
							    [FromServices] IConfiguration config) : this()
        {
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

		public async Task<ResponseData<Product>> GetProductByIdAsync(int id)
		{
			var product = await _repository.GetByIdAsync(id);
            var response = new ResponseData<Product>(product);
            return response;
		}

		public async Task<ResponseData<ListModel<Product>>> GetProductListAsync()
		{
			IReadOnlyList<Product> products;
            
            products = await _repository.ListAllAsync() ;
            
            (int itemsPerPage, int totalPages) = GetPaginationInfo(products.Count);

            var listmodel = new ListModel<Product>(products);
            var responseData = new ResponseData<ListModel<Product>>(listmodel);
			responseData.Data.CurrentPage = -1;
			responseData.Data.TotalPages = totalPages;
            return responseData;
        }

        public async Task<ResponseData<ListModel<Product>>> GetProductListAsync(string categoryNormalizedName)
        {
            IReadOnlyList<Product> products;
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

            (int itemsPerPage, int totalPages) = GetPaginationInfo(products.Count);

            var listmodel = new ListModel<Product>(products);
            var responseData = new ResponseData<ListModel<Product>>(listmodel);
            responseData.Data.CurrentPage = -1;
            responseData.Data.TotalPages = totalPages;
            return responseData;
        }

        public async Task<ResponseData<ListModel<Product>>> GetProductListAsync(int pageNo)
        {
            IReadOnlyList<Product> products;
            products = await _repository.ListAllAsync();

            (int itemsPerPage, int totalPages) = GetPaginationInfo(products.Count);

            if (pageNo > totalPages)
                return new ResponseData<ListModel<Product>>(null, false, "Incorrect page number");

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
        public async Task<ResponseData<ListModel<Product>>> GetProductListAsync(string categoryNormalizedName, int pageNo)
        {
            IReadOnlyList<Product> products;
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

            (int itemsPerPage, int totalPages) = GetPaginationInfo(products.Count);

            if (pageNo > totalPages)
                return new ResponseData<ListModel<Product>>(null, false, "Incorrect page number");

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

        /// <summary>
        /// 
        /// </summary>
        /// <returns>(items per page, total pages)</returns>
        private (int, int) GetPaginationInfo(int productsCount)
        {
            int itemsPerPage;
            try
            {
                itemsPerPage = int.Parse(_config["ItemsPerPage"]);
            }
            catch
            {
                throw new Exception("Unable to receive items per page configuration");
            }
            int totalPages = (int)Math.Ceiling((double)productsCount / itemsPerPage);
            return (itemsPerPage, totalPages);
        }
	}
}
