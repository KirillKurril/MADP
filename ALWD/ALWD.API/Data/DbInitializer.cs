using ALWD.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ALWD.API.Data
{
    public class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
			var baseUri = app.Configuration.GetValue<string>("APIUri") ?? "not founded";
			var rootPath = app.Environment.WebRootPath;

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

			var fileModels = new List<FileModel>
			{
                new FileModel{Name="default-profile-picture.png", URL = baseUri + "Image/default-profile-picture.png", Path=$"{rootPath}\\image\\default-profile-picture.png"},
                new FileModel{Name="стартер_a.jpg", URL = baseUri + "/Image/стартер_a.jpg", MimeType="image/jpeg", Path=$"{rootPath}\\image\\стартер_a.jpg"},
				new FileModel{Name="стартер_b.jpg", URL = baseUri + "/Image/стартер_b.jpg", MimeType="image/jpeg", Path=$"{rootPath}\\image\\стартер_b.jpg"},
				new FileModel{Name="тормозной_диск_a.jpg", URL = baseUri + "/Image/тормозной_диск_a.jpg", MimeType="image/jpeg", Path=$"{rootPath}\\image\\тормозной_диск_a.jpg"},
				new FileModel{Name="тормозной_диск_b.jpg", URL = baseUri + "/Image/тормозной_диск_b.jpg", MimeType="image/jpeg", Path=$"{rootPath}\\image\\тормозной_диск_b.jpg"},
				new FileModel{Name="фильтр_a.jpg", URL = baseUri + "/Image/фильтр_a.jpg", MimeType="image/jpeg", Path=$"{rootPath}\\image\\фильтр_a.jpg"},
				new FileModel{Name="фильтр_b.jpg", URL = baseUri + "/Image/фильтр_b.jpg", MimeType="image/jpeg", Path=$"{rootPath}\\image\\фильтр_b.jpg"},
				new FileModel{Name="свеча_a.jpg", URL = baseUri + "/Image/свеча_a.jpg", MimeType="image/jpeg", Path=$"{rootPath}\\image\\свеча_a.jpg"},
				new FileModel{Name="свеча_b.jpg", URL = baseUri + "/Image/свеча_b.jpg", MimeType="image/jpeg", Path=$"{rootPath}\\image\\свеча_b.jpg"},
				new FileModel{Name="амортизатор_a.jpg", URL = baseUri + "/Image/амортизатор_a.jpg", MimeType="image/jpeg", Path=$"{rootPath}\\image\\амортизатор_a.jpg"},
				new FileModel{Name="амортизатор_b.jpg", URL = baseUri + "/Image/амортизатор_b.jpg", MimeType="image/jpeg", Path=$"{rootPath}\\image\\амортизатор_b.jpg"},
				new FileModel{Name="масло_a.jpg", URL = baseUri + "/Image/масло_a.jpg", MimeType="image/jpeg", Path=$"{rootPath}\\image\\масло_a.jpg"},
				new FileModel{Name="масло_b.jpg", URL = baseUri + "/Image/масло_b.jpg", MimeType="image/jpeg", Path=$"{rootPath}\\image\\масло_b.jpg"},
				new FileModel{Name="шина_a.jpg", URL = baseUri + "/Image/шина_a.jpg", MimeType="image/jpeg", Path=$"{rootPath}\\image\\шина_a.jpg"},
				new FileModel{Name="шина_b.jpg", URL = baseUri + "/Image/шина_b.jpg", MimeType="image/jpeg", Path=$"{rootPath}\\image\\шина_b.jpg"},
				new FileModel{Name="аксессуар_a.jpg", URL = baseUri + "/Image/аксессуар_a.jpg", MimeType="image/jpeg", Path=$"{rootPath}\\image\\аксессуар_a.jpg"},
				new FileModel{Name="аксессуар_b.jpg", URL = baseUri + "/Image/аксессуар_b.jpg", MimeType="image/jpeg", Path=$"{rootPath}\\image\\аксессуар_b.jpg"},
				new FileModel{Name="радиатор_a.jpg", URL = baseUri + "/Image/радиатор_a.jpg", MimeType="image/jpeg", Path=$"{rootPath}\\image\\радиатор_a.jpg"},
				new FileModel{Name="радиатор_b.jpg", URL = baseUri + "/Image/радиатор_b.jpg", MimeType="image/jpeg", Path=$"{rootPath}\\image\\радиатор_b.jpg"},
				new FileModel{Name="бампер_a.jpg", URL = baseUri + "/Image/бампер_a.jpg", MimeType="image/jpeg", Path=$"{rootPath}\\image\\бампер_a.jpg"},
				new FileModel{Name="бампер_b.jpg", URL = baseUri + "/Image/бампер_b.jpg", MimeType="image/jpeg", Path=$"{rootPath}\\image\\бампер_b.jpg"},
			};

			context.FileModels.AddRange(fileModels);

			context.SaveChanges();

			var products = new List<Product>
			{
			new Product {Name="Стартер A", Description="Описание стартера A", Price=120.00, Quantity=10, Image = context.FileModels.Find(2), Category=context.Categories.Find(1)},
			new Product {Name="Стартер B", Description="Описание стартера B", Price=130.00, Quantity=8, Image = context.FileModels.Find(3), Category=context.Categories.Find(1)},

			new Product {Name="Тормозной диск A", Description="Описание тормозного диска A", Price=75.00, Quantity=20, Image = context.FileModels.Find(4),Category=context.Categories.Find(2)},
			new Product {Name="Тормозной диск B", Description="Описание тормозного диска B", Price=80.00, Quantity=15, Image = context.FileModels.Find(5), Category=context.Categories.Find(2)},

			new Product {Name="Фильтр A", Description="Описание фильтра A", Price=15.00, Quantity=50,Image = context.FileModels.Find(6), Category=context.Categories.Find(3)},
			new Product {Name="Фильтр B", Description="Описание фильтра B", Price=18.00, Quantity=40,Image = context.FileModels.Find(7), Category=context.Categories.Find(3)},

			new Product {Name="Свеча зажигания A", Description="Описание свечи A", Price=5.00, Quantity=100,Image = context.FileModels.Find(8), Category=context.Categories.Find(4)},
			new Product {Name="Свеча зажигания B", Description="Описание свечи B", Price=6.00, Quantity=90,Image = context.FileModels.Find(9), Category=context.Categories.Find(4)},

			new Product {Name="Амортизатор A", Description="Описание амортизатора A", Price=50.00, Quantity=30,Image = context.FileModels.Find(10), Category=context.Categories.Find(5)},
			new Product {Name="Амортизатор B", Description="Описание амортизатора B", Price=55.00, Quantity=25,Image = context.FileModels.Find(11), Category=context.Categories.Find(5)},

			new Product {Name="Масло A", Description="Описание масла A", Price=25.00, Quantity=60,Image = context.FileModels.Find(12), Category=context.Categories.Find(6)},
			new Product {Name="Масло B", Description="Описание масла B", Price=30.00, Quantity=55,Image = context.FileModels.Find(13), Category=context.Categories.Find(6)},

			new Product {Name="Шина A", Description="Описание шины A", Price=100.00, Quantity=20,Image = context.FileModels.Find(14), Category=context.Categories.Find(7)},
			new Product {Name="Шина B", Description="Описание шины B", Price=110.00, Quantity=18,Image = context.FileModels.Find(15), Category=context.Categories.Find(7)},

			new Product {Name="Аксессуар A", Description="Описание аксессуара A", Price=20.00, Quantity=70,Image = context.FileModels.Find(16), Category=context.Categories.Find(8)},
			new Product {Name="Аксессуар B", Description="Описание аксессуара B", Price=25.00, Quantity=65,Image = context.FileModels.Find(17), Category=context.Categories.Find(8)},

			new Product {Name="Радиатор A", Description="Описание радиатора A", Price=150.00, Quantity=10,Image = context.FileModels.Find(18), Category=context.Categories.Find(9)},
			new Product {Name="Радиатор B", Description="Описание радиатора B", Price=160.00, Quantity=8,Image = context.FileModels.Find(19), Category=context.Categories.Find(9)},

			new Product {Name="Бампер A", Description="Описание бампера A", Price=200.00, Quantity=5,Image = context.FileModels.Find(120), Category=context.Categories.Find(10)},
			new Product {Name="Бампер B", Description="Описание бампера B", Price=220.00, Quantity=4,Image = context.FileModels.Find(21), Category=context.Categories.Find(10)}
			};
			context.Products.AddRange(products);

			await context.SaveChangesAsync();


        }
    }
}
