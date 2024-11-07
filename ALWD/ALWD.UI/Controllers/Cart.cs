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

		public CartController(ICartService cartService)
		{
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

			if (Request.Headers.ContainsKey("Referer"))
			{
				return Redirect(Request.Headers["Referer"].ToString());
			}

			return RedirectToAction("Index", "Catalog"); 
		}

		[HttpPost("{id}")]
		public IActionResult RemoveFromCart(int id)
		{
			var removeFromCartResponse = _cartService.RemoveFromCart(id);

			if (!removeFromCartResponse.Successfull)
				return StatusCode(500, removeFromCartResponse.ErrorMessage);

			return View("Index", removeFromCartResponse.Data);  
		}

		[HttpPost]
		public IActionResult ClearCart()
		{
			var clearCartResponse = _cartService.ClearCart();

			if (!clearCartResponse.Successfull)
				return StatusCode(500, clearCartResponse.ErrorMessage);

			return View("Index", clearCartResponse.Data);
		}
    }
}
