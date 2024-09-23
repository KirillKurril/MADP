using ALWD.Domain.Entities;
using ALWD.Domain.Models;

namespace ALWD.UI.Services.ProductService
{
	public interface IProductService
	{
		public Task<ResponseData<ListModel<Product>>> GetProductListAsync(string? categoryNormalizedName = null, int pageNo = 1);
		public Task<ResponseData<Product>> GetProductByIdAsync(int id);
		public Task UpdateProductAsync(Product product, IFormFile? formFile);
		public Task DeleteProductAsync(int id);
		public Task CreateProductAsync(Product product, IFormFile? formFile);
	}

}
