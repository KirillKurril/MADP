using Microsoft.EntityFrameworkCore;
using ALWD.API.Data;

namespace ALWD.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
			
            builder.Services.AddScoped<AppDbContext>();
			builder.Services.AddScoped<DbInitializer>();


			var connStr = builder.Configuration
	            .GetConnectionString("ConnectionStrings");

			string dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), connStr);

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connStr)
                .Options;





			builder.Services.AddControllers();
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

			using (var scope = app.Services.CreateScope())
			{
				var dbInitializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
				dbInitializer.SeedData();
			}
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
