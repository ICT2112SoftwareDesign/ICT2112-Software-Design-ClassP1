using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Interfaces;
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

        private readonly IShippingAgent _shippingAgent;
        private readonly IWishlistManagement _wishlistManagement;


        public CustomerPageController(
            ILogger<CustomerPageController> logger,
            CustomerManagement customerManagement,
            SupportManagement supportManagement,
            IChatbot chatbotService,
            OrderManagement orderManagement,
            CartManagement cartManagement,
            IProduct productService,
            IShippingAgent shippingAgent,
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
            _shippingAgent = shippingAgent;
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

        // Support Navigation Methods
        public IActionResult redirectToViewFAQs()
        {
            return RedirectToAction("viewFAQs", "HelpCenterInput");
        }

        public IActionResult redirectToViewSupportTickets()
        {
            return RedirectToAction("viewSupportTickets", "HelpCenterInput");
        }

        // Chatbot Navigation Methods
        public IActionResult redirectToStartChatSession()
        {
            return RedirectToAction("startChatSession", "ChatbotInput");
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
    }
}