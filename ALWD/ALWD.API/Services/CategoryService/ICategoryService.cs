using ALWD.Domain.Entities;
using ALWD.Domain.Models;

namespace ALWD.API.Services.CategoryService
{
    public interface ICategoryService
    {
        public Task<ResponseData<IReadOnlyList<Category>>> GetCategoryListAsync();
        public Task<ResponseData<Category>> GetByNormilizedName(string normilizedName);
        public Task<ResponseData<Category>> GetCategorytByIdAsync(int id);
    }
}
