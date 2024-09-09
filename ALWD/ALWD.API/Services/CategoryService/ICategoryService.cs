using ALWD.Domain.Entities;
using ALWD.Domain.Models;

namespace ALWD.API.Services.CategoryService
{
    public interface ICategoryService
    {
        public Task<ResponseData<List<Category>>> GetCategoryListAsync();
        public Task<Category> GetRandomCategory();
        public Task<Category> GetByNormilizedName(string normilizedName);
    }
}
