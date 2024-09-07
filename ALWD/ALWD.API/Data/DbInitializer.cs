using ALWD.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ALWD.API.Data
{
    public class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
			var imagePath = app.Configuration.GetValue("ImageUri", "not founded");

            using var scope = app.Services.CreateScope();
            var context =
            scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await context.Database.MigrateAsync();

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

			context.Categories.AddRange(categories);

			context.SaveChanges();

			var products = new List<Product>
			{
			new Product { Id = 1, Name="Стартер A", Description="Описание стартера A", Price=120.00, Quantity=10, ImagePath=$"{imagePath}/images/стартер_a.jpg", ImageMimeType="image/jpeg", Category=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("starters"))},
			new Product { Id = 2, Name="Стартер B", Description="Описание стартера B", Price=130.00, Quantity=8, ImagePath=$"{imagePath}/images/стартер_b.jpg", ImageMimeType="image/jpeg", Category=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("starters"))},

			new Product { Id = 3, Name="Тормозной диск A", Description="Описание тормозного диска A", Price=75.00, Quantity=20, ImagePath=$"{imagePath}/images/тормозной_диск_a.jpg", ImageMimeType="image/jpeg", Category=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("brake-discs"))},
			new Product { Id = 4, Name="Тормозной диск B", Description="Описание тормозного диска B", Price=80.00, Quantity=15, ImagePath=$"{imagePath}/images/тормозной_диск_b.jpg", ImageMimeType="image/jpeg", Category=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("brake-discs"))},

			new Product { Id = 5, Name="Фильтр A", Description="Описание фильтра A", Price=15.00, Quantity=50, ImagePath=$"{imagePath}/images/фильтр_a.jpg", ImageMimeType="image/jpeg", CategoryId=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("filters")).Id },
			new Product { Id = 6, Name="Фильтр B", Description="Описание фильтра B", Price=18.00, Quantity=40, ImagePath=$"{imagePath}/images/фильтр_b.jpg", ImageMimeType="image/jpeg", CategoryId=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("filters")).Id },

			new Product { Id = 7, Name="Свеча зажигания A", Description="Описание свечи A", Price=5.00, Quantity=100, ImagePath=$"{imagePath}/images/свеча_a.jpg", ImageMimeType="image/jpeg", CategoryId=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("spark-plugs")).Id },
			new Product { Id = 8, Name="Свеча зажигания B", Description="Описание свечи B", Price=6.00, Quantity=90, ImagePath=$"{imagePath}/images/свеча_b.jpg", ImageMimeType="image/jpeg", CategoryId=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("spark-plugs")).Id },

			new Product { Id = 9, Name="Амортизатор A", Description="Описание амортизатора A", Price=50.00, Quantity=30, ImagePath=$"{imagePath}/images/амортизатор_a.jpg", ImageMimeType="image/jpeg", CategoryId=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("shock-absorbers")).Id },
			new Product { Id = 10, Name="Амортизатор B", Description="Описание амортизатора B", Price=55.00, Quantity=25, ImagePath=$"{imagePath}/images/амортизатор_b.jpg", ImageMimeType="image/jpeg", CategoryId=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("shock-absorbers")).Id },

			new Product { Id = 11, Name="Масло A", Description="Описание масла A", Price=25.00, Quantity=60, ImagePath=$"{imagePath}/images/масло_a.jpg", ImageMimeType="image/jpeg", CategoryId=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("oils-lubricants")).Id },
			new Product { Id = 12, Name="Масло B", Description="Описание масла B", Price=30.00, Quantity=55, ImagePath=$"{imagePath}/images/масло_b.jpg", ImageMimeType="image/jpeg", CategoryId=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("oils-lubricants")).Id },

			new Product { Id = 13, Name="Шина A", Description="Описание шины A", Price=100.00, Quantity=20, ImagePath=$"{imagePath}/images/шина_a.jpg", ImageMimeType="image/jpeg", CategoryId=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("tires-wheels")).Id },
			new Product { Id = 14, Name="Шина B", Description="Описание шины B", Price=110.00, Quantity=18, ImagePath=$"{imagePath}/images/шина_b.jpg", ImageMimeType="image/jpeg", CategoryId=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("tires-wheels")).Id },

			new Product { Id = 15, Name="Аксессуар A", Description="Описание аксессуара A", Price=20.00, Quantity=70, ImagePath=$"{imagePath}/images/аксессуар_a.jpg", ImageMimeType="image/jpeg", CategoryId=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("electronics-accessories")).Id },
			new Product { Id = 16, Name="Аксессуар B", Description="Описание аксессуара B", Price=25.00, Quantity=65, ImagePath=$"{imagePath}/images/аксессуар_b.jpg", ImageMimeType="image/jpeg", CategoryId=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("electronics-accessories")).Id },

			new Product { Id = 17, Name="Радиатор A", Description="Описание радиатора A", Price=150.00, Quantity=10, ImagePath=$"{imagePath}/images/радиатор_a.jpg", ImageMimeType="image/jpeg", CategoryId=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("cooling-system")).Id },
			new Product { Id = 18, Name="Радиатор B", Description="Описание радиатора B", Price=160.00, Quantity=8, ImagePath=$"{imagePath}/images/радиатор_b.jpg", ImageMimeType="image/jpeg", CategoryId=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("cooling-system")).Id },

			new Product { Id = 19, Name="Бампер A", Description="Описание бампера A", Price=200.00, Quantity=5, ImagePath=$"{imagePath}/images/бампер_a.jpg", ImageMimeType="image/jpeg", CategoryId=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("exterior-parts")).Id },
			new Product { Id = 20, Name="Бампер B", Description="Описание бампера B", Price=220.00, Quantity=4, ImagePath=$"{imagePath}/images/бампер_b.jpg", ImageMimeType="image/jpeg", CategoryId=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("exterior-parts")).Id }
			};
			context.Products.AddRange(products);

			await context.SaveChangesAsync();

		}
	}
}
