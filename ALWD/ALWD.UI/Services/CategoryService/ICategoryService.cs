using ALWD.Domain.Entities;
using ALWD.Domain.Models;

namespace ALWD.UI.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<ResponseData<IReadOnlyList<Category>>> GetCategoryListAsync();
        Task<ResponseData<Category>> GetCategoryByIdAsync(int id);
        Task CreateCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);
    }
}
