using ALWD.Domain.Entities;
using ALWD.Domain.Models;
using ALWD.Domain.Validation.Models;
using Microsoft.AspNetCore.Http;

namespace ALWD.Blazor.WebAssembly.Services.ProductService
{
    public interface IProductService
    {
        public Task<ResponseData<ListModel<Product>>> GetProductListAsync(string? categoryNormalizedName = null, int pageNo = 1);
        public Task<ResponseData<Product>> GetProductByIdAsync(int id);
    }

}
