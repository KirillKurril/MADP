using ALWD.Domain.Entities;
using ALWD.Domain.Models;
using ALWD.Domain.Validation.Models;

namespace ALWD.Blazor.WebAssembly.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<ResponseData<IReadOnlyList<Category>>> GetCategoryListAsync();
    }
}
