using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Interfaces;
using System.Text.Json;

namespace CleanBrilliantCompany.Controllers
{
    public class OrderInputController : ApplicationController
    {
        private readonly OrderManagement _orderManagement;
        private readonly CartManagement _cartManagement;
        private readonly IShippingAgents _shippingAgents;
        private readonly ReviewManagement _reviewManagement;
        private readonly CustomerManagement _customerManagement;

        public OrderInputController(
            OrderManagement orderManagement,
            CartManagement cartManagement,
            IShippingAgents shippingAgents,
            ReviewManagement reviewManagement,
            CustomerManagement customerManagement,
            IHttpContextAccessor httpContextAccessor
        ) : base(customerManagement, httpContextAccessor)
        {
            _orderManagement = orderManagement;
            _cartManagement = cartManagement;
            _shippingAgents = shippingAgents;
            _customerManagement = customerManagement;
            _reviewManagement = reviewManagement;
        }

        [HttpGet]
        public IActionResult checkout()
        {
            int? customerId = base.getLoggedInCustomerId();
            if (customerId == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }

            var customerDetails = base.GetCustomerSession();
            string customerAddress = customerDetails?.getSession<string>("customerAddress") ?? string.Empty;

            var cart = _cartManagement.viewCart(customerId.Value);
            if (cart == null || !cart.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("GetAllProducts", "CustomerPage");
            }

            var products = _cartManagement.getCartProductDetails(cart);
            var serviceTypes = _shippingAgents.getServiceTypes();
            var shippingMethods = _shippingAgents.getShippingMethods();

            if (!serviceTypes.Any() || !shippingMethods.Any())
            {
                TempData["Error"] = "No shipping options are available at the moment.";
                return RedirectToAction("GetAllProducts", "CustomerPage");
            }

            var defaultServiceType = serviceTypes.First();
            var shippingAgents = _shippingAgents.getShippingAgentList(Enum.TryParse<Service>(defaultServiceType, out var serviceEnum) ? serviceEnum : Service.OneDay);

            if (!shippingAgents.Any())
            {
                TempData["Error"] = "No shipping agents are available at the moment.";
                return RedirectToAction("GetAllProducts", "CustomerPage");
            }

            var defaultShippingAgent = shippingAgents.First();
            var defaultShippingType = shippingMethods.First();

            decimal cartTotal = _cartManagement.calculateCartTotal(cart, products);
            decimal shippingFee = _orderManagement.calculateShippingFee(defaultServiceType);

            ViewBag.Products = products;
            ViewBag.Cart = cart;
            ViewBag.CartTotal = cartTotal;
            ViewBag.CustomerAddress = customerAddress;
            ViewBag.ServiceTypes = serviceTypes;
            ViewBag.ShippingMethods = shippingMethods;
            ViewBag.ShippingAgents = shippingAgents;
            ViewBag.SelectedServiceType = defaultServiceType;
            ViewBag.SelectedShippingType = defaultShippingType;
            ViewBag.SelectedShippingAgent = defaultShippingAgent;
            ViewBag.ShippingFee = shippingFee;
            ViewBag.FinalTotal = cartTotal + shippingFee;

            return View("~/Views/Order/Checkout.cshtml");
        }

        [HttpPost]
        public IActionResult checkout(string deliveryAddress, string serviceType, string shippingType, string shippingAgent)
        {
            int? customerId = base.getLoggedInCustomerId();
            if (customerId == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }

            var cart = _cartManagement.viewCart(customerId.Value);
            if (cart == null || !cart.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("GetAllProducts", "CustomerPage");
            }

            if (!string.IsNullOrWhiteSpace(deliveryAddress))
            {
                var customer = _customerManagement.getCustomer(customerId.Value);
                customer.setSession("customerAddress", deliveryAddress);
            }
            else
            {
                TempData["Error"] = "Delivery address is required.";
                return RedirectToAction("checkout");
            }

            decimal shippingFee;
            try
            {
                shippingFee = _orderManagement.calculateShippingFee(serviceType);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("checkout");
            }

            var products = _cartManagement.getCartProductDetails(cart);
            decimal cartTotal = cart.Sum(item => Convert.ToDecimal(products[item.Key]["CostPrice"]) * item.Value);

            var shippingAgents = _orderManagement.getAvailableShippingAgents(serviceType);
            var serviceTypes = _orderManagement.getServiceTypes();
            var shippingMethods = _orderManagement.getShippingMethods();

            ViewBag.Products = products;
            ViewBag.Cart = cart;
            ViewBag.CartTotal = cartTotal;
            ViewBag.CustomerAddress = deliveryAddress;
            ViewBag.ServiceTypes = serviceTypes;
            ViewBag.ShippingMethods = shippingMethods;
            ViewBag.ShippingAgents = shippingAgents;
            ViewBag.SelectedServiceType = serviceType;
            ViewBag.SelectedShippingType = shippingType;
            ViewBag.SelectedShippingAgent = shippingAgent;
            ViewBag.ShippingFee = shippingFee;
            ViewBag.FinalTotal = cartTotal + shippingFee;

            return View("~/Views/Order/Checkout.cshtml");
        }

        [HttpPost]
        public IActionResult placeOrder(string deliveryAddress, string serviceType, string shippingType, string shippingAgent)
        {
            int? customerId = base.getLoggedInCustomerId();
            if (customerId == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }

            var cart = _cartManagement.viewCart(customerId.Value);
            if (cart == null || !cart.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("GetAllProducts", "CustomerPage");
            }

            var products = _cartManagement.getCartProductDetails(cart);
            decimal cartTotal = _cartManagement.calculateCartTotal(cart, products);

            decimal shippingFee;
            try
            {
                shippingFee = _orderManagement.calculateShippingFee(serviceType);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("checkout");
            }

            decimal finalTotal = cartTotal + shippingFee;

            ViewBag.CartTotal = cartTotal;
            ViewBag.ShippingFee = shippingFee;
            ViewBag.FinalTotal = finalTotal;
            ViewBag.OrderDetails = new Dictionary<string, string>
            {
                { "DeliveryAddress", deliveryAddress },
                { "serviceType", serviceType },
                { "shippingType", shippingType },
                { "shippingAgent", shippingAgent }
            };

            ViewBag.Cart = cart;

            return View("~/Views/Order/Payment.cshtml");
        }

        [HttpPost]
        public IActionResult processPayment(string deliveryAddress, string serviceType, string shippingType, string shippingAgent,
                                             string cardName, string cardNumber, string expiryDate, string cvv)
        {
            int? customerId = base.getLoggedInCustomerId();
            if (customerId == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }

            var cart = _cartManagement.viewCart(customerId.Value);
            if (cart == null || !cart.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("GetAllProducts", "CustomerPage");
            }

            if (string.IsNullOrWhiteSpace(cardName) || string.IsNullOrWhiteSpace(cardNumber) ||
                string.IsNullOrWhiteSpace(expiryDate) || string.IsNullOrWhiteSpace(cvv))
            {
                TempData["Error"] = "Please enter all payment details.";
                return RedirectToAction("placeOrder", new { deliveryAddress, serviceType, shippingType, shippingAgent });
            }

            var orderId = _orderManagement.createOrder(
                customerId.Value,
                deliveryAddress,
                serviceType,
                shippingType,
                shippingAgent,
                cart
            );

            if (orderId > 0)
            {
                var cartCleared = _cartManagement.clearCart(customerId.Value);
                if (!cartCleared)
                {
                    TempData["Error"] = "Order placed, but failed to clear the cart. Please contact support.";
                }

                TempData["Success"] = "Payment processed and order placed successfully!";
                return RedirectToAction("orderConfirmation", new { orderId });
            }
            else
            {
                TempData["Error"] = "Failed to process payment. Please try again.";
                return RedirectToAction("placeOrder", new { deliveryAddress, serviceType, shippingType, shippingAgent });
            }
        }

        [HttpGet]
        public IActionResult orderConfirmation(int orderId)
        {
            ViewBag.OrderId = orderId;
            return View("~/Views/Order/OrderConfirmation.cshtml");
        }

        [HttpGet]
        public IActionResult toShip()
        {
            int? customerId = base.getLoggedInCustomerId();
            if (customerId == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }

            var orders = _orderManagement.getOrderHistory(customerId.Value)
                                         .Where(o => o.RetrieveStatus() == "Pending")
                                         .ToList();

            var shippingDetails = new Dictionary<int, Dictionary<string, string>>();

            foreach (var order in orders)
            {
                order.UpdateOrderProductsDetails(_cartManagement.getCartProductDetails(order.RetrieveOrderProducts()));

                if (!string.IsNullOrEmpty(order.RetrieveOrderShipping()))
                {
                    try
                    {
                        var details = JsonSerializer.Deserialize<Dictionary<string, string>>(order.RetrieveOrderShipping());
                        if (details != null)
                        {
                            if (details.ContainsKey("ServiceType"))
                            {
                                details["ShippingFee"] = _orderManagement.calculateShippingFee(details["ServiceType"]).ToString("F2");
                            }

                            shippingDetails[order.RetrieveOrderID()] = details;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error deserializing OrderShipping for OrderID {order.RetrieveOrderID()}: {ex.Message}");
                    }
                }
            }

            ViewBag.ShippingDetails = shippingDetails;
            return View("~/Views/Order/ToShip.cshtml", orders);
        }

        [HttpGet]
        public IActionResult toReceive()
        {
            int? customerId = base.getLoggedInCustomerId();
            if (customerId == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }

            var orders = _orderManagement.getOrderHistory(customerId.Value)
                                         .Where(o => o.RetrieveStatus() == "Shipped")
                                         .ToList();

            var shippingDetails = new Dictionary<int, Dictionary<string, string>>();

            foreach (var order in orders)
            {
                order.UpdateOrderProductsDetails(_cartManagement.getCartProductDetails(order.RetrieveOrderProducts()));

                if (!string.IsNullOrEmpty(order.RetrieveOrderShipping()))
                {
                    try
                    {
                        var details = JsonSerializer.Deserialize<Dictionary<string, string>>(order.RetrieveOrderShipping());
                        if (details != null)
                        {
                            if (details.ContainsKey("ServiceType"))
                            {
                                details["ShippingFee"] = _orderManagement.calculateShippingFee(details["ServiceType"]).ToString("F2");
                            }

                            shippingDetails[order.RetrieveOrderID()] = details;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error deserializing OrderShipping for OrderID {order.RetrieveOrderID()}: {ex.Message}");
                    }
                }
            }

            ViewBag.ShippingDetails = shippingDetails;
            return View("~/Views/Order/ToReceive.cshtml", orders);
        }

        [HttpGet]
        public IActionResult completed()
        {
            int? customerId = base.getLoggedInCustomerId();
            if (customerId == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }

            var orders = _orderManagement.getOrderHistory(customerId.Value)
                                         .Where(o => o.RetrieveStatus() == "Completed")
                                         .ToList();

            var reviewedProductIds = _reviewManagement
                .ViewReviewsByCustomer(customerId.Value)
                .Select(r => r.RetrieveProductId())
                .ToHashSet();

            ViewBag.ReviewedProductIds = reviewedProductIds;

            var shippingDetails = new Dictionary<int, Dictionary<string, string>>();

            foreach (var order in orders)
            {
                order.UpdateOrderProductsDetails(_cartManagement.getCartProductDetails(order.RetrieveOrderProducts()));

                if (!string.IsNullOrEmpty(order.RetrieveOrderShipping()))
                {
                    try
                    {
                        var details = JsonSerializer.Deserialize<Dictionary<string, string>>(order.RetrieveOrderShipping());
                        if (details != null)
                        {
                            if (details.ContainsKey("ServiceType"))
                            {
                                details["ShippingFee"] = _orderManagement.calculateShippingFee(details["ServiceType"]).ToString("F2");
                            }

                            shippingDetails[order.RetrieveOrderID()] = details;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error deserializing OrderShipping for OrderID {order.RetrieveOrderID()}: {ex.Message}");
                    }
                }
            }

            ViewBag.ShippingDetails = shippingDetails;
            return View("~/Views/Order/Completed.cshtml", orders);
        }

        [HttpGet]
        public IActionResult cancelled()
        {
            int? customerId = base.getLoggedInCustomerId();
            if (customerId == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }

            var orders = _orderManagement.getOrderHistory(customerId.Value)
                                         .Where(o => o.RetrieveStatus() == "Cancelled")
                                         .ToList();

            var shippingDetails = new Dictionary<int, Dictionary<string, string>>();

            foreach (var order in orders)
            {
                order.UpdateOrderProductsDetails(_cartManagement.getCartProductDetails(order.RetrieveOrderProducts()));

                if (!string.IsNullOrEmpty(order.RetrieveOrderShipping()))
                {
                    try
                    {
                        var details = JsonSerializer.Deserialize<Dictionary<string, string>>(order.RetrieveOrderShipping());
                        if (details != null)
                        {
                            if (details.ContainsKey("ServiceType"))
                            {
                                details["ShippingFee"] = _orderManagement.calculateShippingFee(details["ServiceType"]).ToString("F2");
                            }

                            shippingDetails[order.RetrieveOrderID()] = details;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error deserializing OrderShipping for OrderID {order.RetrieveOrderID()}: {ex.Message}");
                    }
                }
            }

            ViewBag.ShippingDetails = shippingDetails;
            return View("~/Views/Order/Cancelled.cshtml", orders);
        }

        [HttpGet]
        public IActionResult refund()
        {
            int? customerId = base.getLoggedInCustomerId();
            if (customerId == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }

            var orders = _orderManagement.getOrderHistory(customerId.Value)
                                         .Where(o => o.RetrieveStatus() == "Refunded" || o.RetrieveStatus() == "RefundRequested")
                                         .ToList();

            var shippingDetails = new Dictionary<int, Dictionary<string, string>>();

            foreach (var order in orders)
            {
                order.UpdateOrderProductsDetails(_cartManagement.getCartProductDetails(order.RetrieveOrderProducts()));

                if (!string.IsNullOrEmpty(order.RetrieveOrderShipping()))
                {
                    try
                    {
                        var details = JsonSerializer.Deserialize<Dictionary<string, string>>(order.RetrieveOrderShipping());
                        if (details != null)
                        {
                            if (details.ContainsKey("ServiceType"))
                            {
                                details["ShippingFee"] = _orderManagement.calculateShippingFee(details["ServiceType"]).ToString("F2");
                            }

                            shippingDetails[order.RetrieveOrderID()] = details;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error deserializing OrderShipping for OrderID {order.RetrieveOrderID()}: {ex.Message}");
                    }
                }
            }

            ViewBag.ShippingDetails = shippingDetails;
            return View("~/Views/Order/Refund.cshtml", orders);
        }

        [HttpPost]
        public IActionResult cancelOrder(int orderId)
        {
            int? customerId = base.getLoggedInCustomerId();
            if (customerId == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }

            var success = _orderManagement.cancelOrder(orderId, customerId.Value);
            if (success)
            {
                TempData["Success"] = "Order cancelled successfully.";
            }
            else
            {
                TempData["Error"] = "Failed to cancel the order. Please try again.";
            }

            return RedirectToAction("ToShip");
        }

        [HttpPost]
        public IActionResult requestRefund(int orderId, string refundReason, string refundImage, string refundVideo)
        {
            int? customerId = base.getLoggedInCustomerId();
            if (customerId == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }

            var success = _orderManagement.requestRefund(orderId, customerId.Value, refundReason);
            if (success)
            {
                TempData["Success"] = "Refund request submitted successfully.";
            }
            else
            {
                TempData["Error"] = "Failed to submit refund request. Please try again.";
            }

            return RedirectToAction("Completed");
        }
    }
}