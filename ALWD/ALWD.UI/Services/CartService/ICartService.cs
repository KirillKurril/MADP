using ALWD.Domain.Models;

namespace ALWD.UI.Services.CartService
{
	public interface ICartService
	{
        int Count();
        double TotalPrice();
        CartModel GetCart();
		ResponseData<bool> AddToCart(int productId, string productName, double price, int quantity);
		ResponseData<CartModel> RemoveFromCart(int productId);
		ResponseData<CartModel> ClearCart();


    }
}
