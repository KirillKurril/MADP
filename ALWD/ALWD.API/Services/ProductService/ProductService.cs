using ALWD.Domain.Entities;
using ALWD.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using ALWD.Domain.Abstractions;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace ALWD.API.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _repository;
        private IConfiguration _config;
        private readonly int _maxPageSize;
        private readonly string _imagePath;
        private string _apiUri;

        public ProductService(IRepository<Product> repository, [FromServices] IConfiguration config, IWebHostEnvironment env)
        {
            _repository = repository;
            _config = config;
            _imagePath = Path.Combine(env.ContentRootPath, "images");
            _apiUri = _config["ImageUri"];
            try
            {
                _maxPageSize = int.Parse(_config["MaxPageSize"]);
            }
            catch
            {
                throw new Exception("Unable to receive max page size configuration");
            }
        }
        
        public async Task<ResponseData<ListModel<Product>>> GetProductsAsync(int? itemsPerPage, string? categoryNormalizedName, int? pageNo)
        {
            ResponseData<ListModel<Product>> response;
            if (itemsPerPage != null)
            {
                if (pageNo != null && categoryNormalizedName != null)
                    response = await GetProductListAsync(itemsPerPage.Value, categoryNormalizedName, pageNo.Value);

                else if (pageNo != null && categoryNormalizedName == null)
                    response = await GetProductListAsync(itemsPerPage.Value, pageNo.Value);

                else if (pageNo == null && categoryNormalizedName != null)
                    response = await GetProductListAsync(itemsPerPage.Value, categoryNormalizedName);

                else
                    response = await GetProductListAsync(itemsPerPage.Value);
            }
            else
                response = await GetProductListAsync();
            
            return response;
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
        
        public async Task<ResponseData<Product>> CreateProductAsync(Product product, IFormFile? formFile)
        {
            try
            {
                if (product == null)
                {
                    throw new ArgumentNullException(nameof(product));
                }
                product.ImagePath = await SaveFileAsync(formFile);
                await _repository.AddAsync(product);
                return new ResponseData<Product>(product);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the product.", ex);
            }
        }

        public async Task<ResponseData<Product>> UpdateProductAsync(Product product, IFormFile? formFile)
        {
			bool exists = await _repository.Exists(p => p.Id == product.Id);
			if (!exists)
				return new ResponseData<Product>(null, false, "product doesn't exist");

			try
            {
                product.ImagePath = await SaveFileAsync(formFile);
                await _repository.UpdateAsync(product);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the product on product service: {ex.Message}");
            }

			return new ResponseData<Product>(null);

		}
		
        public async Task<ResponseData<Product>> DeleteProductAsync(int id)
        {
            Product product;
            try
            {
                product = await _repository.GetByIdAsync(id);
            }
			catch (Exception ex)
			{
				throw new Exception($"An error occurred while attempting to retrieve the entity to be deleted from the database: {ex.Message}");
			}

			if (product == null)
                return new ResponseData<Product>(null, false, "product doesn't exist");

            try
            {
                await _repository.DeleteAsync(product);
            }
			catch (Exception ex)
			{
				throw new Exception($"An error occurred while deleting the product on product service: {ex.Message}");
			}

			return new ResponseData<Product>(null);
        }

        private async Task<string> SaveFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Пустой файл.");
            }

            var filePath = Path.Combine(_imagePath, file.FileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            string imageUri = Path.Combine(_apiUri, file.FileName);
            return imageUri;
        }
    }
}
