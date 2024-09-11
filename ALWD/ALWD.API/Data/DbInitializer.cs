using ALWD.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ALWD.API.Data
{
    public class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
			var baseUri = app.Configuration.GetValue<string>("ImageUri") ?? "not founded";

			using var scope = app.Services.CreateScope();
            var context =
            scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await context.Database.MigrateAsync();

			var categories = new List<Category>
				 {
					new Category {Name = "Стартеры", NormalizedName = "starters" },
					new Category {Name = "Тормозные диски", NormalizedName = "brake-discs" },
					new Category {Name = "Фильтры", NormalizedName = "filters" },
					new Category {Name = "Свечи зажигания", NormalizedName = "spark-plugs" },
					new Category {Name = "Амортизаторы", NormalizedName = "shock-absorbers" },
					new Category {Name = "Масла и смазки", NormalizedName = "oils-lubricants" },
					new Category {Name = "Шины и диски", NormalizedName = "tires-wheels" },
					new Category {Name = "Электроника и аксессуары", NormalizedName = "electronics-accessories" },
					new Category {Name = "Система охлаждения", NormalizedName = "cooling-system" },
					new Category {Name = "Экстерьерные детали", NormalizedName = "exterior-parts" }
				 };

			context.Categories.AddRange(categories);

			context.SaveChanges();

			var products = new List<Product>
			{
			new Product {Name="Стартер A", Description="Описание стартера A", Price=120.00, Quantity=10, ImagePath=baseUri + "/Image/стартер_a.jpg", ImageMimeType="image/jpeg", Category=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("starters"))},
			new Product {Name="Стартер B", Description="Описание стартера B", Price=130.00, Quantity=8, ImagePath=baseUri + "/Image/стартер_a.jpg", ImageMimeType="image/jpeg", Category=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("starters"))},

			new Product {Name="Тормозной диск A", Description="Описание тормозного диска A", Price=75.00, Quantity=20, ImagePath=baseUri + "/Image/стартер_a.jpg", ImageMimeType="image/jpeg", Category=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("brake-discs"))},
			new Product {Name="Тормозной диск B", Description="Описание тормозного диска B", Price=80.00, Quantity=15, ImagePath=baseUri + "/Image/стартер_a.jpg", ImageMimeType="image/jpeg", Category=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("brake-discs"))},

			new Product {Name="Фильтр A", Description="Описание фильтра A", Price=15.00, Quantity=50, ImagePath=baseUri + "/Image/стартер_a.jpg", ImageMimeType="image/jpeg", CategoryId=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("filters")).Id },
			new Product {Name="Фильтр B", Description="Описание фильтра B", Price=18.00, Quantity=40, ImagePath=baseUri + "/Image/стартер_a.jpg", ImageMimeType="image/jpeg", CategoryId=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("filters")).Id },

			new Product {Name="Свеча зажигания A", Description="Описание свечи A", Price=5.00, Quantity=100, ImagePath=baseUri + "/Image/стартер_a.jpg", ImageMimeType="image/jpeg", CategoryId=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("spark-plugs")).Id },
			new Product {Name="Свеча зажигания B", Description="Описание свечи B", Price=6.00, Quantity=90, ImagePath=baseUri + "/Image/стартер_a.jpg", ImageMimeType="image/jpeg", CategoryId=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("spark-plugs")).Id },

			new Product {Name="Амортизатор A", Description="Описание амортизатора A", Price=50.00, Quantity=30, ImagePath=baseUri + "/Image/стартер_a.jpg", ImageMimeType="image/jpeg", CategoryId=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("shock-absorbers")).Id },
			new Product {Name="Амортизатор B", Description="Описание амортизатора B", Price=55.00, Quantity=25, ImagePath=baseUri + "/Image/стартер_a.jpg", ImageMimeType="image/jpeg", CategoryId=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("shock-absorbers")).Id },

			new Product {Name="Масло A", Description="Описание масла A", Price=25.00, Quantity=60, ImagePath=baseUri + "/Image/стартер_a.jpg", ImageMimeType="image/jpeg", CategoryId=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("oils-lubricants")).Id },
			new Product {Name="Масло B", Description="Описание масла B", Price=30.00, Quantity=55, ImagePath=baseUri + "/Image/стартер_a.jpg", ImageMimeType="image/jpeg", CategoryId=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("oils-lubricants")).Id },

			new Product {Name="Шина A", Description="Описание шины A", Price=100.00, Quantity=20, ImagePath=baseUri + "/Image/стартер_a.jpg", ImageMimeType="image/jpeg", CategoryId=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("tires-wheels")).Id },
			new Product {Name="Шина B", Description="Описание шины B", Price=110.00, Quantity=18, ImagePath=baseUri + "/Image/стартер_a.jpg", ImageMimeType="image/jpeg", CategoryId=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("tires-wheels")).Id },

			new Product {Name="Аксессуар A", Description="Описание аксессуара A", Price=20.00, Quantity=70, ImagePath=baseUri + "/Image/стартер_a.jpg", ImageMimeType="image/jpeg", CategoryId=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("electronics-accessories")).Id },
			new Product {Name="Аксессуар B", Description="Описание аксессуара B", Price=25.00, Quantity=65, ImagePath=baseUri + "/Image/стартер_a.jpg", ImageMimeType="image/jpeg", CategoryId=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("electronics-accessories")).Id },

			new Product {Name="Радиатор A", Description="Описание радиатора A", Price=150.00, Quantity=10, ImagePath=baseUri + "/Image/стартер_a.jpg", ImageMimeType="image/jpeg", CategoryId=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("cooling-system")).Id },
			new Product {Name="Радиатор B", Description="Описание радиатора B", Price=160.00, Quantity=8, ImagePath=baseUri + "/Image/стартер_a.jpg", ImageMimeType="image/jpeg", CategoryId=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("cooling-system")).Id },

			new Product {Name="Бампер A", Description="Описание бампера A", Price=200.00, Quantity=5, ImagePath=baseUri + "/Image/стартер_a.jpg", ImageMimeType="image/jpeg", CategoryId=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("exterior-parts")).Id },
			new Product {Name="Бампер B", Description="Описание бампера B", Price=220.00, Quantity=4, ImagePath=baseUri + "/Image/стартер_a.jpg", ImageMimeType="image/jpeg", CategoryId=context.Categories.FirstOrDefault(c=>c.NormalizedName.Equals("exterior-parts")).Id }
			};
			context.Products.AddRange(products);

			await context.SaveChangesAsync();


        }
    }
}
