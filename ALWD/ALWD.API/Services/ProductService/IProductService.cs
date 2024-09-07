using ALWD.Domain.Entities;
using ALWD.Domain.Models;

namespace ADLW1.Services.ProductService
{
	public interface IProductService
	{
		public Task<ResponseData<ListModel<Product>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1);
		public Task<ResponseData<Product>> GetProductByIdAsync(int id);
		public Task UpdateProductAsync(int id, Product product, IFormFile? formFile);
		public Task DeleteProductAsync(int id);
		public Task<ResponseData<Product>> CreateProductAsync(Product product, IFormFile? formFile);
	}

}
