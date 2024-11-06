using Microsoft.AspNetCore.Mvc;
using ALWD.UI.Services.CartService;
using Microsoft.AspNetCore.Authorization;
using ALWD.API.Services.ProductService;

namespace ALWD.UI.Controllers
{
	[Authorize]
	public class CartController : Controller
	{
		readonly ICartService _cartService;
		readonly IProductService _productService;

		public CartController(ICartService cartService, IProductService productService)
		{
			_productService = productService;
			_cartService = cartService;
		}

		public IActionResult Index()
		{
			var cart = _cartService.GetCart();
			return View(cart);
		}

		[HttpPost]
		public IActionResult AddToCart(int productId, string productName, double productPrice, int productQuantity)
		{
			var addToCartResponse = _cartService.AddToCart(productId, productName, productPrice, productQuantity);

			if (!addToCartResponse.Successfull)
				return StatusCode(500, addToCartResponse.ErrorMessage);

			return RedirectToAction("Index"); 
		}

		[HttpDelete("{id}")]
		public IActionResult RemoveFromCart(int productId)
		{
			var removeFromCartResponse = _cartService.RemoveFromCart(productId);

			if (!removeFromCartResponse.Successfull)
				return StatusCode(500, removeFromCartResponse.ErrorMessage);

			return RedirectToAction("Index");  
		}

		[HttpDelete]
		public IActionResult ClearCart()
		{
			var clearCartResponse = _cartService.ClearCart();

			if (!clearCartResponse.Successfull)
				return StatusCode(500, clearCartResponse.ErrorMessage);

			return RedirectToAction("Index");
		}
	}
}
