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
			Database.EnsureCreated();
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Category>()
				.HasMany(c => c.Products)
				.WithOne(p => p.Category)
				.HasForeignKey(p => p.CategoryId);
		}

	}
}
