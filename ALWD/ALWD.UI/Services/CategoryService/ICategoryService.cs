﻿using ALWD.Domain.Entities;
using ALWD.Domain.Models;
using ALWD.Domain.Validation.Models;

namespace ALWD.UI.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<ResponseData<IReadOnlyList<Category>>> GetCategoryListAsync();
        Task<ResponseData<Category>> GetCategoryByIdAsync(int id);
        Task<ResponseData<int>> CreateCategoryAsync(CategoryCreateValidationModel model);
        Task<ResponseData<int>> UpdateCategoryAsync(CategoryEditValidationModel model);
        Task DeleteCategoryAsync(int id);
    }
}
