using ALWD.Domain.Abstractions;
using ALWD.Domain.Entities;
using ALWD.Domain.Models;
using System.Collections.Generic;

namespace ALWD.API.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private IRepository<Category> _repository;
        public CategoryService(IRepository<Category> repository) 
            => _repository = repository;
        public async Task<ResponseData<IReadOnlyList<Category>>> GetCategoryListAsync()
        {
            IReadOnlyList<Category> categories = await _repository.ListAllAsync();
            ResponseData<IReadOnlyList<Category>> response  = new ResponseData<IReadOnlyList<Category>>(categories);
            return response;
        }

        public async Task<ResponseData<Category>> GetByNormilizedName(string normilizedName)
        {
            Category category = await _repository.FirstOrDefaultAsync(
                c  => c.NormalizedName == normilizedName);

            ResponseData<Category> response = new ResponseData<Category>(category);
            return response;
        }

        public async Task<ResponseData<Category>> GetCategorytByIdAsync(int id)
        {
            Category category = await _repository.GetByIdAsync(id);
            ResponseData<Category> response = new ResponseData<Category>(category);
            return response;
        }
    }

}


