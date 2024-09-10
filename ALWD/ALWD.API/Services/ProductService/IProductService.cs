using ALWD.Domain.Entities;
using ALWD.Domain.Models;

namespace ALWD.API.Services.ProductService
{
	public interface IProductService
	{
        public Task<ResponseData<ListModel<Product>>> GetProductListAsync();
        public Task<ResponseData<ListModel<Product>>> GetProductListAsync(int itemsPerPage);
        public Task<ResponseData<ListModel<Product>>> GetProductListAsync(int itemsPerPage, int pageNo);
        public Task<ResponseData<ListModel<Product>>> GetProductListAsync(int itemsPerPage, string categoryNormalizedName);
        public Task<ResponseData<ListModel<Product>>> GetProductListAsync(int itemsPerPage, string? categoryNormalizedName, int pageNo);
        public Task<ResponseData<Product>> GetProductByIdAsync(int id);
		public Task<ResponseData<Product>> UpdateProductAsync(int id, Product product, IFormFile? formFile);
		public Task<ResponseData<Product>> DeleteProductAsync(int id);
		public Task<ResponseData<Product>> CreateProductAsync(Product product, IFormFile? formFile);
	}

}
