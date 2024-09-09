using ALWD.UI.Services.CategoryService;
using ALWD.UI.Services.ProductService;

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
