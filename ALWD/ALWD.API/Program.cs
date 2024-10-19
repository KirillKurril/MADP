using Microsoft.EntityFrameworkCore;
using ALWD.API.Data;
using ALWD.Domain.Abstractions;
using ALWD.Domain.Entities;
using ALWD.API.Data.Repository;
using ALWD.API.Services.ProductService;
using ALWD.API.Services.CategoryService;
using ALWD.API.Services.FileService;
using System.Text.Json.Serialization;
using ALWD.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ALWD.API.Services.AccountService;

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
			var connStr = builder.Configuration.GetConnectionString("MicrosoftSQLServer");
			builder.Services.AddDbContext<AppDbContext>(options =>
				options.UseSqlServer(connStr));

            builder.Services.AddHttpClient();

            builder.Services.AddScoped<IRepository<Product>, EfRepository<Product>>();
			builder.Services.AddScoped<IRepository<Category>, EfRepository<Category>>();
			builder.Services.AddScoped<IRepository<FileModel>, EfRepository<FileModel>>();
			builder.Services.AddScoped<IProductService, ProductService>();
			builder.Services.AddScoped<ICategoryService, CategoryService>();
			builder.Services.AddScoped<IFileService, FileService>();
			builder.Services.AddScoped<IAccountService, KeycloakAccountService>();

			builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var authServer = builder.Configuration
			.GetSection("AuthServer")
			.Get<AuthServerData>();
			
			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
			{
				o.MetadataAddress = $"{authServer.Host}/realms/{authServer.Realm}/.wellknown/openid-configuration";
				o.Authority = $"{authServer.Host}/realms/{authServer.Realm}";
				o.Audience = "account";
				o.RequireHttpsMetadata = false;
			});

			builder.Services.AddAuthorization(opt =>
			{
				opt.AddPolicy("admin", p => p.RequireRole("POWER-USER"));
			});
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
			app.UseAuthentication();
			app.UseAuthorization();

		}

		private static void ConfigureEndpoints(WebApplication app)
		{
			app.MapControllers();
		}
	}
}
