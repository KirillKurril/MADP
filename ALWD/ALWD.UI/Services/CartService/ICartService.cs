using ALWD.Domain.Models;

namespace ALWD.UI.Services.CartService
{
	public interface ICartService
	{
		CartModel GetCart();
		ResponseData<bool> AddToCart(int productId, string productName, double price, int quantity);
		ResponseData<bool> RemoveFromCart(int productId);
		ResponseData<bool> ClearCart();

	}
}
