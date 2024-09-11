using ALWD.Domain.Entities;
using ALWD.Domain.Models;

namespace ALWD.UI.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<ResponseData<IReadOnlyList<Category>>> GetCategoryListAsync();
        Task<ResponseData<Category>> GetCategoryByIdAsync(int id);
        Task<ResponseData<Category>> CreateCategoryAsync(Category category);
        Task<ResponseData<Category>> UpdateCategoryAsync(int id, Category category);
        Task<ResponseData<bool>> DeleteCategoryAsync(int id);
    }
}
