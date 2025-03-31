using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Interfaces;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using CleanBrilliantCompany.Models.SupportTicket;

namespace CleanBrilliantCompany.Controllers
{
    public class CustomerPageController : ApplicationController
    {
        private readonly ILogger<CustomerPageController> _logger;
        private readonly CustomerManagement _customerManagement;
        private readonly SupportManagement _supportManagement;
        private readonly IChatbot _chatbotService;
        private readonly OrderManagement _orderManagement;
        private readonly CartManagement _cartManagement;
        private readonly IProduct _productService;

        private readonly ReviewManagement _reviewManagement;

        private readonly IShippingAgents _shippingAgents;
        private readonly IWishlistManagement _wishlistManagement;


        public CustomerPageController(
            ILogger<CustomerPageController> logger,
            CustomerManagement customerManagement,
            SupportManagement supportManagement,
            IChatbot chatbotService,
            OrderManagement orderManagement,
            CartManagement cartManagement,
            IProduct productService,
            IShippingAgents shippingAgents,
            ReviewManagement reviewManagement,
            IWishlistManagement wishlistManagement, 
            IHttpContextAccessor httpContextAccessor
            ) : base(customerManagement, httpContextAccessor)
        {
            _logger = logger;
            _customerManagement = customerManagement;
            _supportManagement = supportManagement;
            _chatbotService = chatbotService;
            _orderManagement = orderManagement;
            _cartManagement = cartManagement;
            _productService = productService;
            _shippingAgents = shippingAgents;
            _reviewManagement = reviewManagement;
            _wishlistManagement = wishlistManagement;
        } 

        // View return is done here
        // Customer Pages
        public IActionResult CustomerDetails()
        {
            ViewBag.CustomerId = TempData["CustomerId"];
            ViewBag.Email = TempData["Email"];
            ViewBag.Password = TempData["Password"];
            ViewBag.Username = TempData["Username"];
            ViewBag.CustomerAddress = TempData["CustomerAddress"];
            ViewBag.Message = TempData["Message"];

            return View("~/Views/CustomerPage/Profile/CustomerDetails.cshtml");
        }



        // INPUT CONTROLLER METHODS

        // HelpCenterInputController Methods

        public IActionResult viewFAQs()
        {
            Dictionary<string, string> faqs = _supportManagement.FetchFAQs();
            ViewBag.FAQs = faqs;

            return View("~/Views/Support/FAQs.cshtml");
        }

        public IActionResult escalateIssue(String issueDescription)
        {
            // Retrieve customer ID from the session using the correct key
            int? customerId = base.getLoggedInCustomerId();
            if (customerId == -1)
            {
                TempData["Error"] = "User not logged in.";
                return Json(new { redirectUrl = Url.Action("Login", "BeforeLoginPage") });
            }

            bool success = _supportManagement.createSupportTicket(customerId ?? -1, issueDescription);
            if (success)
            {
                Console.WriteLine("Support ticket created, pop up will appear!");
                TempData["Success"] = "Issue has been successfully raised!";
            } else{
                TempData["Error"] = "Failed to raise the issue!";
            }

            return RedirectToAction("viewFAQs");
        }

        public IActionResult viewSupportTickets()
        {
            // Retrieve customer ID from the session using the correct key
            int? customerId = base.getLoggedInCustomerId();
            if (customerId == -1)
            {
                TempData["Error"] = "User not logged in.";
                return Json(new { redirectUrl = Url.Action("Login", "BeforeLoginPage") });
            }

            var allCustomerSupportTicket = _supportManagement.viewTicketByCustomer(customerId ?? -1);
            ViewBag.AllSupportTickets = allCustomerSupportTicket;        
            return View("~/Views/Support/ViewSupportTickets.cshtml");
        }

        // ChatbotInputController Methods

        public IActionResult startChatSession()
        {
            string chatHistoryJson = HttpContext.Session.GetString("ChatHistory");

            // Check if JSON exists and is not empty
            List<Dictionary<string, string>> chatHistory = !string.IsNullOrWhiteSpace(chatHistoryJson)
                ? JsonSerializer.Deserialize<List<Dictionary<string, string>>>(chatHistoryJson)
                : new List<Dictionary<string, string>>();

            ViewBag.ChatHistory = chatHistory;

            return View("~/Views/Support/Chatbot.cshtml");
        }

        [HttpPost]
        public IActionResult provideAutomatedResponse(String query)
        {
            // Retrieve customer ID from the session using the correct key
            int? customerId = base.getLoggedInCustomerId();
            if (customerId == -1)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }

            if (string.IsNullOrEmpty(query)) return RedirectToAction("startChatSession");

            string botResponse = _supportManagement.handleQuery(customerId ?? -1, query);

            string chatHistoryJson = HttpContext.Session.GetString("ChatHistory");

            // Check if JSON is null or empty before deserialization
            List<Dictionary<string, string>> chatHistory = !string.IsNullOrWhiteSpace(chatHistoryJson)
                ? JsonSerializer.Deserialize<List<Dictionary<string, string>>>(chatHistoryJson)
                : new List<Dictionary<string, string>>();

            var userMessage = new Dictionary<string, string> { { "user", query } };
            var botMessage = new Dictionary<string, string> { { "bot", botResponse } };

            chatHistory.Add(userMessage);
            chatHistory.Add(botMessage);

            // Store updated chat history back in session
            HttpContext.Session.SetString("ChatHistory", JsonSerializer.Serialize(chatHistory));


            return RedirectToAction("startChatSession");
        }

        public IActionResult GetAllProducts(string query = "", string filters = "All", string sortOrder = "asc")
        {
            List<Product> products = _productService.getAllProducts();

            // Apply search filter
            if (!string.IsNullOrEmpty(query))
            {
                products = products.Where(p => p.GetProductDetails()["ProductName"].ToString().Contains(query, System.StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Apply category filter
            if (filters != "All")
            {
                products = products.Where(p => p.GetProductDetails()["Category"].ToString() == filters).ToList();
            }

            // Apply sorting
            products = sortOrder == "asc"
                ? products.OrderBy(p => float.Parse(p.GetProductDetails()["CostPrice"].ToString())).ToList()
                : products.OrderByDescending(p => float.Parse(p.GetProductDetails()["CostPrice"].ToString())).ToList();

            // Convert to a list of dictionaries
            var productDetails = products.Select(product => product.GetProductDetails()).ToList();

            return View("~/Views/Products/Index.cshtml", productDetails);
        }

        public List<Product> FilterProducts(List<string> categories)
        {
            var allProducts = _productService.getAllProducts();
            return allProducts.FindAll(p => categories.Contains(p.GetProductDetails()["Category"].ToString()));
        }

        [HttpGet]
        public IActionResult ProductDetail(int productId)
        {
            var product = _productService.getProductDetails(productId);
            if (product == null)
            {
                TempData["Error"] = "Product not found.";
                return RedirectToAction("GetAllProducts");
            }

            var productDetails = product.GetProductDetails();

            var reviews = _reviewManagement.ViewReviewsByProduct(productId);

            ViewBag.ProductReviews = reviews;
            ViewBag.ProductId = productId;
            return View("~/Views/Products/ProductDetails.cshtml", productDetails);
        }


        //Cart Navigation Methods

        public IActionResult redirectToCart()
        {
            return RedirectToAction("viewCart", "CartInput");
        }
        
        // ORDER Navigation Methods

        // Redirect to the Checkout page
        public IActionResult redirectToCheckout()
        {
            return RedirectToAction("checkout", "OrderInput");
        }

        // Redirect to the Place Order page
        public IActionResult redirectToPlaceOrder()
        {
            return RedirectToAction("placeOrder", "OrderInput");
        }

        // Redirect to the Order Confirmation page
        public IActionResult redirectToOrderConfirmation(int orderId)
        {
            return RedirectToAction("orderConfirmation", "OrderInput", new { orderId = orderId });
        }

        // Redirect to the "To Ship" orders page
        public IActionResult redirectToToShip()
        {
            return RedirectToAction("toShip", "OrderInput");
        }

        // Redirect to the "To Receive" orders page
        public IActionResult redirectToToReceive()
        {
            return RedirectToAction("toReceive", "OrderInput");
        }

        // Redirect to the Completed Orders page
        public IActionResult redirectToCompletedOrders()
        {
            return RedirectToAction("completed", "OrderInput");
        }

        // Redirect to the Cancelled Orders page
        public IActionResult RedirectToCancelledOrders()
        {
            return RedirectToAction("Cancelled", "OrderInput");
        }

        // Redirect to the Refund Orders page
        public IActionResult RedirectToRefundOrders()
        {
            return RedirectToAction("Refund", "OrderInput");
        }
        
        //REVIEW INPUT CONTROLLER METHODS 
        /*
        [HttpGet]
        public IActionResult RateProduct(int productId)
        {
            var product = _productService.getProductDetails(productId);
            if (product == null)
            {
                TempData["Error"] = "Product Not found";
                return RedirectToAction("Completed");
            }
            ViewBag.ProductId = productId;
            ViewBag.ProductName = product.GetProductDetails()["ProductName"];

            return View("~/Views/Review/RateProduct.cshtml");
        }

        [HttpGet]
        public IActionResult EditReview(int productId)
        {
            int? customerId = base.getLoggedInCustomerId();
            if (customerId == null)
                return RedirectToAction("Login", "BeforeLoginPage");

            var review = _reviewManagement
                .ViewReviewsByCustomer(customerId.Value)
                .FirstOrDefault(r => r.RetrieveProductId() == productId);

            if (review == null)
            {
                TempData["Error"] = "Review not found.";
                return RedirectToAction("Completed");
            }

            var product = _productService.getProductDetails(productId);

            ViewBag.ProductId = productId;
            ViewBag.ProductName = product?.GetProductDetails()["ProductName"];
            ViewBag.ReviewText = review.RetrieveReviewText();
            ViewBag.Rating = review.RetrieveRating();
            ViewBag.ReviewId = review.RetrieveReviewId();

            return View("~/Views/Review/RateProduct.cshtml");
        }

        [HttpPost]
        public IActionResult SubmitReview(string reviewText, int rating, int productId)
        {
            Console.WriteLine($"Review: {reviewText}, Rating: {rating}, ProductID: {productId}");
            if (!_reviewManagement.WriteReview(reviewText, rating, productId))
            {
                TempData["Error"] = "Failed to submit review. Make sure all fields are valid.";
            }
            else
            {
                TempData["Success"] = "Review submitted successfully!";
            }

            return RedirectToAction("Completed"); // gotta check where to go next. 
        }
        [HttpPost]
        public IActionResult SubmitEditedReview(int reviewId, string reviewText, int rating, int productId)
        {
            if (!_reviewManagement.EditReview(reviewId, reviewText, rating))
            {
                TempData["Error"] = "Failed to update review.";
            }
            else
            {
                TempData["Success"] = "Review updated successfully!";
            }

            return RedirectToAction("Completed");
        }


        [HttpPost]
        public IActionResult EditReview(int reviewId, string reviewText, int rating)
        {
            if (!_reviewManagement.EditReview(reviewId, reviewText, rating))
            {
                TempData["Error"] = "Failed to edit review. Please try again.";
            }
            else
            {
                TempData["Success"] = "Review updated successfully!";
            }

            return RedirectToAction("GetAllProducts");
        }
        [HttpPost]
        public IActionResult DeleteReview(int reviewId)
        {
            Console.WriteLine($"Deleting review with ID: {reviewId}");

            if (!_reviewManagement.DeleteReview(reviewId))
            {
                TempData["Error"] = "Failed to delete review.";
            }
            else
            {
                TempData["Success"] = "Review deleted successfully.";
            }

            return RedirectToAction("Completed");
        }

        //[HttpPost]
        //public IActionResult RemoveReview(int reviewId)
        //{
        //   if (!_reviewManagement.DeleteReview(reviewId))
        //   {
        //      TempData["Error"] = "Failed to delete review.";
        //  }
        //   else
        //  {
        //        TempData["Success"] = "Review deleted successfully!";
        //   }

        //   return RedirectToAction("GetAllProducts");
        // }

        [HttpGet]
        public IActionResult ViewReviews()
        {
            var reviews = _reviewManagement.ViewReviews();
            return View("~/Views/Review/ReviewHTML.cshtml", reviews);
        }

        [HttpGet]
        public IActionResult ViewReviewsByProduct(int productId)
        {
            var product = _productService.getProductDetails(productId);
            if (product == null)
            {
                TempData["Error"] = "Product not found.";
                return RedirectToAction("GetAllProducts");
            }

            var reviews = _reviewManagement.ViewReviewsByProduct(productId);

            ViewBag.ProductName = product.GetProductDetails()["ProductName"];
            ViewBag.ProductId = productId;

            return View("~/Views/Review/ProductReviews.cshtml", reviews);
        }*/

        // part of ProductInputController
        [HttpPost]
        public IActionResult AddToWishlist(int productId)
        {
            // Retrieve customer ID from the session
            int? customerId = base.getLoggedInCustomerId();
            if (customerId == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }


            // Call the wishlist management service
            var success = _wishlistManagement.addToWishlist(customerId.Value, productId);

            if (success)
            {
                TempData["Success"] = "Product added to wishlist successfully!";
            }
            else
            {
                TempData["Info"] = "Product is already in your wishlist.";
            }

            return RedirectToAction("GetAllProducts");
        }

        // part of ProductInputController
        public IActionResult viewWishlist()
        {
            // Retrieve customer ID from the session
            int? customerId = base.getLoggedInCustomerId();
            if (customerId == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }


            // Get customer details to retrieve the name
            var customer = _customerManagement.getCustomer(customerId.Value);
            var customerDetails = base.GetCustomerSession();
            
            string customerName = customerDetails?.getSession<string>("username") ?? "My";
            // Get wishlist items from wishlist management service
            var productIds = _wishlistManagement.viewWishlist(customerId.Value);

            // Get detailed product information for each wishlist item
            var wishlistProducts = new Dictionary<int, Dictionary<string, object>>();

            foreach (var productId in productIds)
            {
                var product = _productService.getProductDetails(productId);
                if (product != null)
                {
                    wishlistProducts.Add(productId, product.GetProductDetails());
                }
            }
            // Pass the customer name to the view
            ViewBag.CustomerName = customerName;
            return View("~/Views/Wishlist/WishlistIndex.cshtml", wishlistProducts);
        }

        [HttpPost]
        public IActionResult removeFromWishlist(int productId)
        {
            // Retrieve customer ID from the session
            int? customerId = base.getLoggedInCustomerId();
            if (customerId == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }

            // Call the wishlist management service
            var success = _wishlistManagement.removeFromWishlist(customerId.Value, productId);

            if (success)
            {
                TempData["Success"] = "Product removed from wishlist successfully!";
            }
            else
            {
                TempData["Error"] = "Failed to remove product from wishlist.";
            }

            return RedirectToAction("viewWishlist");
        }

        [HttpPost]
        public IActionResult updatePreferences(int customerId, bool suppressPaid, bool suppressCancelled)
        {
            var customer = _customerManagement.getCustomer(customerId);
            if (customer != null)
            {
                customer.SetPreferencesFromCheckbox(suppressPaid, suppressCancelled);
                _customerManagement.updateEmailPreference(customerId, customer.getEmailPreferenceRaw());
                Console.WriteLine("Email Preference: " + customer.getEmailPreferenceRaw());

                // 🆕 Refresh session
                var updatedCustomer = _customerManagement.getCustomer(customerId);
                HttpContext.Session.SetString("emailPreference", updatedCustomer.getEmailPreferenceRaw() ?? "");
            }

            return RedirectToAction("CustomerDetails", "CustomerInput");
        }

        

    }
}