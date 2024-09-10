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
        private readonly int _maxPageSize;

        public ProductService(IRepository<Product> repository,
                                [FromServices] IConfiguration config) : this()
        {
            _repository = repository;
            _config = config;
            try
            {
                _maxPageSize = int.Parse(_config["MaxPageSize"]);
            }
            catch
            {
                throw new Exception("Unable to receive max page size configuration");
            }
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

            products = await _repository.ListAllAsync();

            int itemsPerPage = products.Count;
            int totalPages = -1;

            var listmodel = new ListModel<Product>(products);
            var responseData = new ResponseData<ListModel<Product>>(listmodel);
            responseData.Data.CurrentPage = -1;
            responseData.Data.TotalPages = totalPages;
            return responseData;
        }
        public async Task<ResponseData<ListModel<Product>>> GetProductListAsync(int itemsPerPage)
        {
            IReadOnlyList<Product> products;

            if (itemsPerPage > _maxPageSize)
                itemsPerPage = _maxPageSize;

            products = await _repository.ListAllAsync();

            int totalPages = (int)Math.Ceiling((double)products.Count / itemsPerPage);

            var listmodel = new ListModel<Product>(products);
            var responseData = new ResponseData<ListModel<Product>>(listmodel);
            responseData.Data.CurrentPage = -1;
            responseData.Data.TotalPages = totalPages;
            return responseData;
        }

        public async Task<ResponseData<ListModel<Product>>> GetProductListAsync(int itemsPerPage, string categoryNormalizedName)
        {
            IReadOnlyList<Product> products;

            if (itemsPerPage > _maxPageSize)
                itemsPerPage = _maxPageSize;

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

            int totalPages = (int)Math.Ceiling((double)products.Count / itemsPerPage);

            var listmodel = new ListModel<Product>(products);
            var responseData = new ResponseData<ListModel<Product>>(listmodel);
            responseData.Data.CurrentPage = -1;
            responseData.Data.TotalPages = totalPages;
            return responseData;
        }

        public async Task<ResponseData<ListModel<Product>>> GetProductListAsync(int itemsPerPage, int pageNo)
        {
            IReadOnlyList<Product> products;

            if (itemsPerPage > _maxPageSize)
                itemsPerPage = _maxPageSize;

            products = await _repository.ListAllAsync();

            int totalPages = (int)Math.Ceiling((double)products.Count / itemsPerPage);

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
        public async Task<ResponseData<ListModel<Product>>> GetProductListAsync(int itemsPerPage, string categoryNormalizedName, int pageNo)
        {
            if (itemsPerPage > _maxPageSize)
                itemsPerPage = _maxPageSize;

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

            int totalPages = (int)Math.Ceiling((double)products.Count / itemsPerPage);

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
    }
}
