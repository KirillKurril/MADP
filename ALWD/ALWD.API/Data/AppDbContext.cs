using ALWD.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ALWD.API.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
        {
			//Database.EnsureDeleted();
			//после этой строки конструктор принципиально отказывается работать, даже если эту строку запихнуть в другой метод
			Database.EnsureCreated();
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Product>()
				.HasOne(p => p.Category)
				.WithMany(c => c.Products);
		}
	}
}
