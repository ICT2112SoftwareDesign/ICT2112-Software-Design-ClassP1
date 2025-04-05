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
        
        public IActionResult redirectToEscalateIssue(string issueDescription)
        {
            return RedirectToAction("escalateIssue", "HelpCenterInput",  new { issueDescription = issueDescription });
        }

        // Chatbot Navigation Methods
        public IActionResult redirectToStartChatSession()
        {
            return RedirectToAction("startChatSession", "ChatbotInput");
        }

        public IActionResult redirectToProvideAutomatedResponse(string query)
        {
            return RedirectToAction("provideAutomatedResponse", "ChatbotInput",  new { query });
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

        [HttpPost]
        public IActionResult redirectToDeleteReview(int reviewId) 
        { 
            Console.WriteLine($"Redirecting to delete review with ID: {reviewId}");

            return RedirectToAction("DeleteReview", "ReviewInput",new { reviewId = reviewId });
        }

        [HttpPost]
        public IActionResult redirectToSubmitReview(string reviewText, int rating, int productId)
        {
            return RedirectToAction("SubmitReview", "ReviewInput", new { reviewText = reviewText, rating = rating, productId = productId });
        }

        [HttpPost]
        public IActionResult redirectToSubmitEditedReview(int reviewId, string reviewText, int rating, int productId)
        {
            return RedirectToAction("SubmitEditedReview", "ReviewInput", new { reviewId = reviewId, reviewText = reviewText, rating = rating, productId = productId });
        }


        [HttpPost]
        public IActionResult redirectToViewReviewsByProduct(int productId)
        {
            return RedirectToAction("ViewReviewsByProduct", "ReviewInput", new { productId = productId });

        }
    }
}