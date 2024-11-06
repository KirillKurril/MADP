

namespace ALWD.Domain.Models
{
	public class CartItem
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public double Price { get; set; }
		public int Count { get; set; }
	}
}
