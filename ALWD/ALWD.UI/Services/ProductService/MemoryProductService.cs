﻿using ALWD.UI.Services.CategoryService;
using ALWD.Domain.Entities;
using ALWD.Domain.Models;

namespace ADLW1.Services.ProductService
{
	public class MemoryProductService : IProductService
	{
		List<Product> _products;
		List<Category> _categories;
		public MemoryProductService(ICategoryService categoryService)
		{
			_categories = categoryService.GetCategoryListAsync()
		   .Result
		   .Data;
			SetupData();
		}

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

		public Task<ResponseData<ListModel<Product>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
		{
			List<Product> products = _products.Where(p => p.Category.NormalizedName == categoryNormalizedName).ToList();
			var listmodel = new ListModel<Product>(_products);
			var resposeData = new ResponseData<ListModel<Product>>(listmodel);
			return Task.FromResult(resposeData);
		}

		public Task UpdateProductAsync(int id, Product product, IFormFile? formFile)
		{
			throw new NotImplementedException();
		}

		private void SetupData()
		{
			_productes = new List<Product>
		{
			new Product { Id = 1, Name="Стартер A", Description="Описание стартера A", Price=120.00, Quantity=10, ImagePath="Images/стартер_a.jpg", ImageMimeType="image/jpeg", Category=_categories.Find(c=>c.NormalizedName.Equals("starters")) },
			new Product { Id = 2, Name="Стартер B", Description="Описание стартера B", Price=130.00, Quantity=8, ImagePath="Images/стартер_b.jpg", ImageMimeType="image/jpeg", Category=_categories.Find(c=>c.NormalizedName.Equals("starters")) },

			new Product { Id = 3, Name="Тормозной диск A", Description="Описание тормозного диска A", Price=75.00, Quantity=20, ImagePath="Images/тормозной_диск_a.jpg", ImageMimeType="image/jpeg", Category=_categories.Find(c=>c.NormalizedName.Equals("brake-discs")) },
			new Product { Id = 4, Name="Тормозной диск B", Description="Описание тормозного диска B", Price=80.00, Quantity=15, ImagePath="Images/тормозной_диск_b.jpg", ImageMimeType="image/jpeg", Category=_categories.Find(c=>c.NormalizedName.Equals("brake-discs")) },

			new Product { Id = 5, Name="Фильтр A", Description="Описание фильтра A", Price=15.00, Quantity=50, ImagePath="Images/фильтр_a.jpg", ImageMimeType="image/jpeg", Category=_categories.Find(c=>c.NormalizedName.Equals("filters")) },
			new Product { Id = 6, Name="Фильтр B", Description="Описание фильтра B", Price=18.00, Quantity=40, ImagePath="Images/фильтр_b.jpg", ImageMimeType="image/jpeg", Category=_categories.Find(c=>c.NormalizedName.Equals("filters")) },

			new Product { Id = 7, Name="Свеча зажигания A", Description="Описание свечи A", Price=5.00, Quantity=100, ImagePath="Images/свеча_a.jpg", ImageMimeType="image/jpeg", Category=_categories.Find(c=>c.NormalizedName.Equals("spark-plugs")) },
			new Product { Id = 8, Name="Свеча зажигания B", Description="Описание свечи B", Price=6.00, Quantity=90, ImagePath="Images/свеча_b.jpg", ImageMimeType="image/jpeg", Category=_categories.Find(c=>c.NormalizedName.Equals("spark-plugs")) },

			new Product { Id = 9, Name="Амортизатор A", Description="Описание амортизатора A", Price=50.00, Quantity=30, ImagePath="Images/амортизатор_a.jpg", ImageMimeType="image/jpeg", Category=_categories.Find(c=>c.NormalizedName.Equals("shock-absorbers")) },
			new Product { Id = 10, Name="Амортизатор B", Description="Описание амортизатора B", Price=55.00, Quantity=25, ImagePath="Images/амортизатор_b.jpg", ImageMimeType="image/jpeg", Category=_categories.Find(c=>c.NormalizedName.Equals("shock-absorbers")) },

			new Product { Id = 11, Name="Масло A", Description="Описание масла A", Price=25.00, Quantity=60, ImagePath="Images/масло_a.jpg", ImageMimeType="image/jpeg", Category=_categories.Find(c=>c.NormalizedName.Equals("oils-lubricants")) },
			new Product { Id = 12, Name="Масло B", Description="Описание масла B", Price=30.00, Quantity=55, ImagePath="Images/масло_b.jpg", ImageMimeType="image/jpeg", Category=_categories.Find(c=>c.NormalizedName.Equals("oils-lubricants")) },

			new Product { Id = 13, Name="Шина A", Description="Описание шины A", Price=100.00, Quantity=20, ImagePath="Images/шина_a.jpg", ImageMimeType="image/jpeg", Category=_categories.Find(c=>c.NormalizedName.Equals("tires-wheels")) },
			new Product { Id = 14, Name="Шина B", Description="Описание шины B", Price=110.00, Quantity=18, ImagePath="Images/шина_b.jpg", ImageMimeType="image/jpeg", Category=_categories.Find(c=>c.NormalizedName.Equals("tires-wheels")) },

			new Product { Id = 15, Name="Аксессуар A", Description="Описание аксессуара A", Price=20.00, Quantity=70, ImagePath="Images/аксессуар_a.jpg", ImageMimeType="image/jpeg", Category=_categories.Find(c=>c.NormalizedName.Equals("electronics-accessories")) },
			new Product { Id = 16, Name="Аксессуар B", Description="Описание аксессуара B", Price=25.00, Quantity=65, ImagePath="Images/аксессуар_b.jpg", ImageMimeType="image/jpeg", Category=_categories.Find(c=>c.NormalizedName.Equals("electronics-accessories")) },

			new Product { Id = 17, Name="Радиатор A", Description="Описание радиатора A", Price=150.00, Quantity=10, ImagePath="Images/радиатор_a.jpg", ImageMimeType="image/jpeg", Category=_categories.Find(c=>c.NormalizedName.Equals("cooling-system")) },
			new Product { Id = 18, Name="Радиатор B", Description="Описание радиатора B", Price=160.00, Quantity=8, ImagePath="Images/радиатор_b.jpg", ImageMimeType="image/jpeg", Category=_categories.Find(c=>c.NormalizedName.Equals("cooling-system")) },

			new Product { Id = 19, Name="Бампер A", Description="Описание бампера A", Price=200.00, Quantity=5, ImagePath="Images/бампер_a.jpg", ImageMimeType="image/jpeg", Category=_categories.Find(c=>c.NormalizedName.Equals("exterior-parts")) },
			new Product { Id = 20, Name="Бампер B", Description="Описание бампера B", Price=220.00, Quantity=4, ImagePath="Images/бампер_b.jpg", ImageMimeType="image/jpeg", Category=_categories.Find(c=>c.NormalizedName.Equals("exterior-parts")) }
		};
		}
	}
}