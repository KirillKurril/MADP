
namespace ALWD.Domain.Models
{
	public class CartModel
	{
		public Dictionary<int, CartItem> Items { get; set; }
		public int Count
			=> Items.Count;
		public double TotalPrice
			=> Items.Values.Sum(i => i.Price * i.Count);
		public void AddItem(CartItem item)
			=> Items.Add(item.Id, item);
		public void RemoveItem(int itemId)
			=> Items.Remove(itemId);
	}
}
