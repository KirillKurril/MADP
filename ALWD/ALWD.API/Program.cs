using Microsoft.EntityFrameworkCore;
using ALWD.API.Data;
using ALWD.Domain.Abstractions;
using ALWD.Domain.Entities;
using ALWD.API.Data.Repository;
using ALWD.API.Services.ProductService;
using ALWD.API.Services.CategoryService;
using ALWD.API.Services.FileService;
using System.Text.Json.Serialization;

namespace ALWD.API
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			ConfigureServices(builder);
			var app = builder.Build();

			await InitializeDatabase(app);
			ConfigureMiddleware(app);
			ConfigureEndpoints(app);

			app.Run();
		}

		private static void ConfigureServices(WebApplicationBuilder builder)
		{
			var connStr = builder.Configuration.GetConnectionString("Default");
			builder.Services.AddDbContext<AppDbContext>(options =>
				options.UseSqlite(connStr));

			builder.Services.AddScoped<IRepository<Product>, EfRepository<Product>>();
			builder.Services.AddScoped<IRepository<Category>, EfRepository<Category>>();
			builder.Services.AddScoped<IRepository<FileModel>, EfRepository<FileModel>>();
			builder.Services.AddScoped<IProductService, ProductService>();
			builder.Services.AddScoped<ICategoryService, CategoryService>();
			builder.Services.AddScoped<IFileService, FileService>();

			builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
		}

		private static async Task InitializeDatabase(WebApplication app)
		{
			using var scope = app.Services.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();

			await DbInitializer.SeedData(app);
		}

		private static void ConfigureMiddleware(WebApplication app)
		{
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();
			app.UseAuthorization();
		}

		private static void ConfigureEndpoints(WebApplication app)
		{
			app.MapControllers();
		}
	}
}
