using ALWD.Domain.Entities;
using ALWD.Domain.Models;
using ALWD.UI.Services.CategoryService;

namespace ADLW1.Services.CategoryService
{
    public class MemoryCategoryService : ICategoryService
    {
        public Task<ResponseData<List<Category>>>
       GetCategoryListAsync()
        {
            var categories = new List<Category>
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
            var result = new ResponseData<List<Category>>();
            result.Data = categories;
            return Task.FromResult(result); //как это работает 
        }
    }

}
