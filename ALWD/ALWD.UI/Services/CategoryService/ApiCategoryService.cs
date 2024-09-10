using ALWD.Domain.Entities;
using ALWD.Domain.Models;

namespace ALWD.UI.Services.CategoryService
{
    public class ApiCategoryService : ICategoryService
    {
        public Task<Category> GetByNormilizedName(string normilizedName)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Category> GetRandomCategory()
        {
            throw new NotImplementedException();
        }
    }
}
