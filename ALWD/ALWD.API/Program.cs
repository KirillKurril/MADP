using Microsoft.EntityFrameworkCore;
using ALWD.API.Data;

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

            // Нет необходимости регистрировать DbInitializer, так как он статический.
            // Убираем его из DI-контейнера.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            var scope = app.Services.CreateScope();
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
