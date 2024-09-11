using Microsoft.EntityFrameworkCore;
using ALWD.API.Data;
using ALWD.Domain.Abstractions;
using ALWD.Domain.Entities;
using ALWD.API.Data.Repository;
using ALWD.API.Services.ProductService;
using ALWD.API.Services.CategoryService;

namespace ALWD.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connStr = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connStr));

            builder.Services.AddScoped<IRepository<Product>, EfRepository<Product>>();
			builder.Services.AddScoped<IRepository<Category>, EfRepository<Category>>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();

			builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();


            using var scope = app.Services.CreateScope();
            var context =
            scope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            DbInitializer.SeedData(app);
            // Асинхронная инициализация базы данных


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
