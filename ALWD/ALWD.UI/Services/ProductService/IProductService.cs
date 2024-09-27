using ALWD.Domain.Entities;
using ALWD.Domain.Models;
using ALWD.Domain.Validation.Models;

namespace ALWD.UI.Services.ProductService
{
	public interface IProductService
	{
		public Task<ResponseData<ListModel<Product>>> GetProductListAsync(string? categoryNormalizedName = null, int pageNo = 1);
		public Task<ResponseData<Product>> GetProductByIdAsync(int id);
        public Task<ResponseData<int>> CreateProductAsync(Product product, IFormFile? formFile);
        public Task<ResponseData<int>> CreateProductAsync(ProductCreateValidationModel model);
        public Task<ResponseData<int>> UpdateProductAsync(Product product, IFormFile? formFile);
        public Task<ResponseData<int>> UpdateProductAsync(ProductEditValidationModel model);
        public Task<ResponseData<bool>> DeleteProductAsync(int id);
	}

}
