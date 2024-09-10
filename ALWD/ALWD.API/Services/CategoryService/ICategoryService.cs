using ALWD.Domain.Entities;
using ALWD.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace ALWD.API.Services.CategoryService
{
    public interface ICategoryService
    {
        public Task<ResponseData<IReadOnlyList<Category>>> GetCategoryListAsync();
        public Task<ResponseData<Category>> GetByNormilizedName(string normilizedName);
        public Task<ResponseData<Category>> GetCategoryByIdAsync(int id);
        public Task<ResponseData<Category>> CreateCategoryAsync(Category category);
        public Task<ResponseData<Category>> UpdateCategoryAsync(int id, Category category);
        public Task<ResponseData<Category>> DeleteCategoryAsync(int id);

    }
}
