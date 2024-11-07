using ALWD.Domain.Models;
using Azure;
using Newtonsoft.Json;

namespace ALWD.UI.Services.CartService
{
    public class SessionCartService : ICartService
	{
		readonly IHttpContextAccessor _httpContextAccessor;
		const string CartSessionKey = "cart";

        public SessionCartService(IHttpContextAccessor httpContextAccessor)
			=> _httpContextAccessor = httpContextAccessor;

		public int Count()
		{
            var cart = GetCart();
			return cart.Count;
        }
        public double TotalPrice()
		{
            var cart = GetCart();
            return cart.TotalPrice;
        }
        public CartModel GetCart()
		{
			var cart = _httpContextAccessor.HttpContext!.Session.GetString(CartSessionKey);
			return cart == null
				? new CartModel()
				: JsonConvert.DeserializeObject<CartModel>(cart)!;
		}
		private ResponseData<bool> SaveCart(CartModel cart)
		{
			try
			{
				_httpContextAccessor.HttpContext!.Session.SetString(CartSessionKey, JsonConvert.SerializeObject(cart));
			}
			catch (Exception ex)
			{
				return new ResponseData<bool>(false, false, ex.Message);
			}
			return new ResponseData<bool>(true);
		}
		public ResponseData<bool> AddToCart(int productId, string productName, double price, int quantity)
		{
			try
			{
				var cart = GetCart();
				cart.AddItem(new CartItem
				{
					Id = productId,
					Name = productName,
					Price = price,
					Count = quantity
				});
				var saveCartResponse = SaveCart(cart);
				if (!saveCartResponse.Successfull)
					return saveCartResponse;
			}
			catch (Exception ex)
			{
				return new ResponseData<bool>(false, false, ex.Message);
			}
			return new ResponseData<bool>(true);

		}
		public ResponseData<CartModel> RemoveFromCart(int productId)
		{
			CartModel? cart = null;
			try
			{
				cart = GetCart();
				cart.RemoveItem(productId);
				var saveCartResponse = SaveCart(cart);
				if (!saveCartResponse.Successfull)
					return new ResponseData<CartModel>(null, false, saveCartResponse.ErrorMessage!);
			}
			catch (Exception ex)
			{
				return new ResponseData<CartModel>(cart, false, ex.Message);
			}
			return new ResponseData<CartModel>(cart);
		}
		public ResponseData<CartModel> ClearCart()
		{
			CartModel? cart = null;
			try
			{
				cart = new CartModel();
				var saveCartResponse = SaveCart(cart);
				if (!saveCartResponse.Successfull)
					return new ResponseData<CartModel>(null, false, saveCartResponse.ErrorMessage!);
			}
			catch (Exception ex)
			{
				return new ResponseData<CartModel>(cart, false, ex.Message);
			}
			return new ResponseData<CartModel>(cart);
		}
	}
}
