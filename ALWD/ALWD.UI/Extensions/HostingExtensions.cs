using ADLW1.Services.CategoryService;
using ADLW1.Services.ProductService;
using ALWD.UI.Services.CategoryService;

namespace ALWD.UI.Extensions
{
    public static class HostingExtensions
    {
        public static void RegisterCustomServices(
               this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICategoryService, MemoryCategoryService>();
            builder.Services.AddScoped<IProductService, MemoryProductService>();
        }
    }
}
