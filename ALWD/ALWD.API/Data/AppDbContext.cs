using ALWD.Domain.Entities;
using ALWD.Domain.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ALWD.API.Data
{
	public class AppDbContext : DbContext
	{
		public DbSet<Product> Products { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<FileModel> FileModels { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
        {
			Database.EnsureCreated();
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Product>()
				.HasOne(p => p.Category)
				.WithMany();

			modelBuilder.Entity<Product>()
				.HasOne(p => p.Image)
				.WithMany();

		}
	}
}
