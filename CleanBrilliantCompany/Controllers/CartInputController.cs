using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Controllers
{
    public class CartInputController : ApplicationController 
    {
        private readonly CartManagement _cartManagement;
        private readonly CustomerManagement _customerManagement;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartInputController(CartManagement cartManagement, CustomerManagement customerManagement, IHttpContextAccessor httpContextAccessor)
        : base(customerManagement, httpContextAccessor) 
        {
            _cartManagement = cartManagement;
            _httpContextAccessor = httpContextAccessor;
            _customerManagement = customerManagement; 
        }

        [HttpPost]
        public IActionResult addToCart(int productId, int quantity)
        {
            int? customerId = base.getLoggedInCustomerId();
            if (customerId == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }

            if (quantity <= 0)
            {
                TempData["Error"] = "Quantity must be greater than zero.";
                return RedirectToAction("viewProducts", "ProductInput");
            }

            var success = _cartManagement.addToCart(customerId.Value, productId, quantity);
            TempData[success ? "Success" : "Error"] = success ? "Product added to cart successfully!" : "Failed to add product to cart.";

            return RedirectToAction("viewProducts", "ProductInput");
        }

        [HttpPost]
        public IActionResult updateQuantity(int productId, int quantity)
        {
            int? customerId = base.getLoggedInCustomerId();
            if (customerId == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }

            if (quantity <= 0)
            {
                TempData["Error"] = "Quantity must be greater than zero.";
                return RedirectToAction("ViewCart");
            }

            var success = _cartManagement.updateQuantity(customerId.Value, productId, quantity);
            TempData[success ? "Success" : "Error"] = success ? "Cart updated successfully." : "Failed to update cart.";

            return RedirectToAction("ViewCart");
        }

        [HttpPost]
        public IActionResult removeFromCart(int productId)
        {
            int? customerId = base.getLoggedInCustomerId();
            if (customerId == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }

            var success = _cartManagement.removeFromCart(customerId.Value, productId);
            TempData[success ? "Success" : "Error"] = success ? "Product removed from cart successfully." : "Failed to remove product from cart.";

            return RedirectToAction("ViewCart");
        }

        public IActionResult viewCart()
        {
            int? customerId = base.getLoggedInCustomerId();
            if (customerId == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }

            var cartData = _cartManagement.viewCart(customerId.Value);
            var products = _cartManagement.getCartProductDetails(cartData);
            var cartTotal = _cartManagement.calculateCartTotal(cartData, products);

            ViewBag.Products = products;
            ViewBag.Total = cartTotal;

            return View("~/Views/Cart/Cart.cshtml", cartData);
        }
    }
}