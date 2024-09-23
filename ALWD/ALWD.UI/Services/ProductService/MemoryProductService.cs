using ALWD.UI.Services.CategoryService;
using ALWD.Domain.Entities;
using ALWD.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace ALWD.UI.Services.ProductService
{
	public class MemoryProductService : IProductService
	{
        private List<Product> _products = new();
        private IReadOnlyList<Category> _categories;
		ICategoryService _categoryService;
		private IConfiguration _config;
        public MemoryProductService(
									[FromServices] IConfiguration config,
							        ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _config = config;
            SetupData();

        }


        public int PageNo { get; }

        public Task<ResponseData<Product>> CreateProductAsync(Product product, IFormFile? formFile)
		{
			throw new NotImplementedException();
		}

		public Task DeleteProductAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<ResponseData<Product>> GetProductByIdAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<ResponseData<ListModel<Product>>> GetProductListAsync(string? categoryNormalizedName, int pageNo)
		{
			List<Product> products = new();

            if (string.IsNullOrEmpty(categoryNormalizedName)) 
			{
				products = _products;
			}
			else
			{
                try
                {
                    products = _products.Where(p => p.Category.NormalizedName == categoryNormalizedName).ToList();
                }
                catch (Exception ex)
                {
                    var failResponseData = new ResponseData<ListModel<Product>>(
                        new ListModel<Product>(), false, "Product list receiving fault");
                    return Task.FromResult(failResponseData);
                }
            }
            int itemsPerPage;
            try
            {
                itemsPerPage = int.Parse(_config["ItemsPerPage"]);
            }
            catch
            {
                throw new Exception("Unable to receive items per page configuration");
            }
            int totalPages = (int)Math.Ceiling((double)products.Count / itemsPerPage);

            var productsOnPage = products
                .Skip((pageNo - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToList();


            var listmodel = new ListModel<Product>(productsOnPage);
            var responseData = new ResponseData<ListModel<Product>>(listmodel);
			responseData.Data.CurrentPage = pageNo;
			responseData.Data.TotalPages = totalPages;
            return Task.FromResult(responseData);

        }

		public Task UpdateProductAsync(int id, Product product, IFormFile? formFile)
		{
			throw new NotImplementedException();
		}

        Task IProductService.CreateProductAsync(Product product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        Task IProductService.DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<ResponseData<Product>> IProductService.GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<ResponseData<ListModel<Product>>> IProductService.GetProductListAsync(string? categoryNormalizedName, int pageNo)
        {
            throw new NotImplementedException();
        }

        private async void SetupData()
			{
				var categoriesResponse = await _categoryService.GetCategoryListAsync();
				_categories = categoriesResponse.Data;
				_products = new List<Product>
			{
			new Product { Id = 1, Name="Стартер A", Description="Описание стартера A", Price=120.00, Quantity=10, ImagePath="/images/стартер_a.jpg", ImageMimeType="image/jpeg", Category=_categories.FirstOrDefault(c=>c.NormalizedName.Equals("starters")) },
			new Product { Id = 2, Name="Стартер B", Description="Описание стартера B", Price=130.00, Quantity=8, ImagePath="/images/стартер_b.jpg", ImageMimeType="image/jpeg", Category=_categories.FirstOrDefault(c=>c.NormalizedName.Equals("starters")) },

			new Product { Id = 3, Name="Тормозной диск A", Description="Описание тормозного диска A", Price=75.00, Quantity=20, ImagePath="/images/тормозной_диск_a.jpg", ImageMimeType="image/jpeg", Category=_categories.FirstOrDefault(c=>c.NormalizedName.Equals("brake-discs")) },
			new Product { Id = 4, Name="Тормозной диск B", Description="Описание тормозного диска B", Price=80.00, Quantity=15, ImagePath="/images/тормозной_диск_b.jpg", ImageMimeType="image/jpeg", Category=_categories.FirstOrDefault(c=>c.NormalizedName.Equals("brake-discs")) },

			new Product { Id = 5, Name="Фильтр A", Description="Описание фильтра A", Price=15.00, Quantity=50, ImagePath="/images/фильтр_a.jpg", ImageMimeType="image/jpeg", Category=_categories.FirstOrDefault(c=>c.NormalizedName.Equals("filters")) },
			new Product { Id = 6, Name="Фильтр B", Description="Описание фильтра B", Price=18.00, Quantity=40, ImagePath="/images/фильтр_b.jpg", ImageMimeType="image/jpeg", Category=_categories.FirstOrDefault(c=>c.NormalizedName.Equals("filters")) },

			new Product { Id = 7, Name="Свеча зажигания A", Description="Описание свечи A", Price=5.00, Quantity=100, ImagePath="/images/свеча_a.jpg", ImageMimeType="image/jpeg", Category=_categories.FirstOrDefault(c=>c.NormalizedName.Equals("spark-plugs")) },
			new Product { Id = 8, Name="Свеча зажигания B", Description="Описание свечи B", Price=6.00, Quantity=90, ImagePath="/images/свеча_b.jpg", ImageMimeType="image/jpeg", Category=_categories.FirstOrDefault(c=>c.NormalizedName.Equals("spark-plugs")) },

			new Product { Id = 9, Name="Амортизатор A", Description="Описание амортизатора A", Price=50.00, Quantity=30, ImagePath="/images/амортизатор_a.jpg", ImageMimeType="image/jpeg", Category=_categories.FirstOrDefault(c=>c.NormalizedName.Equals("shock-absorbers")) },
			new Product { Id = 10, Name="Амортизатор B", Description="Описание амортизатора B", Price=55.00, Quantity=25, ImagePath="/images/амортизатор_b.jpg", ImageMimeType="image/jpeg", Category=_categories.FirstOrDefault(c=>c.NormalizedName.Equals("shock-absorbers")) },

			new Product { Id = 11, Name="Масло A", Description="Описание масла A", Price=25.00, Quantity=60, ImagePath="/images/масло_a.jpg", ImageMimeType="image/jpeg", Category=_categories.FirstOrDefault(c=>c.NormalizedName.Equals("oils-lubricants")) },
			new Product { Id = 12, Name="Масло B", Description="Описание масла B", Price=30.00, Quantity=55, ImagePath="/images/масло_b.jpg", ImageMimeType="image/jpeg", Category=_categories.FirstOrDefault(c=>c.NormalizedName.Equals("oils-lubricants")) },

			new Product { Id = 13, Name="Шина A", Description="Описание шины A", Price=100.00, Quantity=20, ImagePath="/images/шина_a.jpg", ImageMimeType="image/jpeg", Category=_categories.FirstOrDefault(c=>c.NormalizedName.Equals("tires-wheels")) },
			new Product { Id = 14, Name="Шина B", Description="Описание шины B", Price=110.00, Quantity=18, ImagePath="/images/шина_b.jpg", ImageMimeType="image/jpeg", Category=_categories.FirstOrDefault(c=>c.NormalizedName.Equals("tires-wheels")) },

			new Product { Id = 15, Name="Аксессуар A", Description="Описание аксессуара A", Price=20.00, Quantity=70, ImagePath="/images/аксессуар_a.jpg", ImageMimeType="image/jpeg", Category=_categories.FirstOrDefault(c=>c.NormalizedName.Equals("electronics-accessories")) },
			new Product { Id = 16, Name="Аксессуар B", Description="Описание аксессуара B", Price=25.00, Quantity=65, ImagePath="/images/аксессуар_b.jpg", ImageMimeType="image/jpeg", Category=_categories.FirstOrDefault(c=>c.NormalizedName.Equals("electronics-accessories")) },

			new Product { Id = 17, Name="Радиатор A", Description="Описание радиатора A", Price=150.00, Quantity=10, ImagePath="/images/радиатор_a.jpg", ImageMimeType="image/jpeg", Category=_categories.FirstOrDefault(c=>c.NormalizedName.Equals("cooling-system")) },
			new Product { Id = 18, Name="Радиатор B", Description="Описание радиатора B", Price=160.00, Quantity=8, ImagePath="/images/радиатор_b.jpg", ImageMimeType="image/jpeg", Category=_categories.FirstOrDefault(c=>c.NormalizedName.Equals("cooling-system")) },

			new Product { Id = 19, Name="Бампер A", Description="Описание бампера A", Price=200.00, Quantity=5, ImagePath="/images/бампер_a.jpg", ImageMimeType="image/jpeg", Category=_categories.FirstOrDefault(c=>c.NormalizedName.Equals("exterior-parts")) },
			new Product { Id = 20, Name="Бампер B", Description="Описание бампера B", Price=220.00, Quantity=4, ImagePath="/images/бампер_b.jpg", ImageMimeType="image/jpeg", Category=_categories.FirstOrDefault(c=>c.NormalizedName.Equals("exterior-parts")) }
		};

			}

        Task IProductService.UpdateProductAsync(Product product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}
