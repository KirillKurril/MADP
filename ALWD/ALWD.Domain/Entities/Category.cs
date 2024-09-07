
namespace ALWD.Domain.Entities
{
	public class Category : DbEntity
	{
		public string Name { get; set; } = null!;
		public string NormalizedName { get; set; } = null!;
		public List<Product>? Products { get; set; }
	}
}
