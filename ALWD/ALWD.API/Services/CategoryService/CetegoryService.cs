using ALWD.Domain.Entities;
using ALWD.Domain.Models;

namespace ALWD.API.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private List<Category> _categories;
        public Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var result = new ResponseData<List<Category>>(_categories);
            return Task.FromResult(result); //как это работает 
        }

        public Task<Category> GetRandomCategory()
        {
            int categoryIndex = new Random().Next(0, _categories.Count);
            try
            {
                Category category = _categories[categoryIndex];
                return Task.FromResult(category);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new Exception("Collections category is empty");
            }
        }

        public Task<Category> GetByNormilizedName(string normilizedName)
        {
            var category = _categories.Find(c => c.NormalizedName == normilizedName);
            return Task.FromResult(category);
        }
    }

}


