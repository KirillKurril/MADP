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

        public async Task<ResponseData<Category>> GetCategoryByIdAsync(int id)
        {
            Category category = await _repository.GetByIdAsync(id);
            ResponseData<Category> response = new ResponseData<Category>(category);
            return response;
        }

        public async Task<ResponseData<Category>> CreateCategoryAsync(Category category)
        {
            /////нужно ли проверять существует ли уже такое
            await _repository.AddAsync(category);
            return new ResponseData<Category>(category);
        }

        public async Task<ResponseData<Category>> UpdateCategoryAsync(int id, Category category)
        {
            if (await _repository.GetByIdAsync(id) == null)
                return new ResponseData<Category>(null, false, "category doesn't exist");

            await _repository.UpdateAsync(category);
            return new ResponseData<Category>(null);

        }

        public async Task<ResponseData<Category>> DeleteCategoryAsync(int id)
        {
            Category category = await _repository.GetByIdAsync(id);
            if (category == null)
                return new ResponseData<Category>(null, false, "category doesn't exist");

            await _repository.DeleteAsync(category);
            return new ResponseData<Category>(null);
        }
    }

}


