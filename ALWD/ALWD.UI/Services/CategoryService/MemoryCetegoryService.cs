using ALWD.Domain.Entities;
using ALWD.Domain.Models;
using ALWD.UI.Services.CategoryService;

namespace ADLW1.Services.CategoryService
{
    public class MemoryCategoryService : ICategoryService
    {
        private List<Category> _categories;

        public MemoryCategoryService() 
            => SetupData();
        public Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var result = new ResponseData<List<Category>>(_categories);
            return Task.FromResult(result); //как это работает 
        }

        public void SetupData()
        {
            _categories = new List<Category>
                 {
                    new Category { Id = 1, Name = "Стартеры", NormalizedName = "starters" },
                    new Category { Id = 2, Name = "Тормозные диски", NormalizedName = "brake-discs" },
                    new Category { Id = 3, Name = "Фильтры", NormalizedName = "filters" },
                    new Category { Id = 4, Name = "Свечи зажигания", NormalizedName = "spark-plugs" },
                    new Category { Id = 5, Name = "Амортизаторы", NormalizedName = "shock-absorbers" },
                    new Category { Id = 6, Name = "Масла и смазки", NormalizedName = "oils-lubricants" },
                    new Category { Id = 7, Name = "Шины и диски", NormalizedName = "tires-wheels" },
                    new Category { Id = 8, Name = "Электроника и аксессуары", NormalizedName = "electronics-accessories" },
                    new Category { Id = 9, Name = "Система охлаждения", NormalizedName = "cooling-system" },
                    new Category { Id = 10, Name = "Экстерьерные детали", NormalizedName = "exterior-parts" }
                 };
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
