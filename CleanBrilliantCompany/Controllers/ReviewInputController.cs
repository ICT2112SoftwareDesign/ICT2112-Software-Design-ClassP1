using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CleanBrilliantCompany.Models;
namespace CleanBrilliantCompany.Controllers
{ 
    public class ReviewInputController : ApplicationController
    { 
        private readonly ReviewManagement _reviewManagement;
        private readonly CustomerManagement _customerManagement;

       public ReviewInputController(
        ReviewManagement reviewManagement,
        CustomerManagement customerManagement,
        IHttpContextAccessor httpContextAccessor
    ) : base(customerManagement, httpContextAccessor)
    {
        _reviewManagement = reviewManagement;
        _customerManagement = customerManagement;
    }

        //REVIEW INPUT CONTROLLER METHODS 
        [HttpGet]
        public IActionResult RateProduct(int productId)
        {
            var product = _reviewManagement.GetProductName(productId);
            if (product == null)
            {
                TempData["Error"] = "Product Not found";
                return RedirectToAction("Completed");
            }
            ViewBag.ProductId = productId;
            //ViewBag.ProductName = product.GetProductDetails()["ProductName"];
            ViewBag.ProductName = product; 
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

            //var product = _productService.getProductDetails(productId);
            var product = _reviewManagement.GetProductName(productId);

            ViewBag.ProductId = productId;
            ViewBag.ProductName = product; 

            //ViewBag.ProductName = product?.GetProductDetails()["ProductName"];
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

            return RedirectToAction("Completed", "Customerpage"); // gotta check where to go next. 
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

            return RedirectToAction("Completed", "CustomerPage");
        }

        /*
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
        }*/
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

            return RedirectToAction("Completed", "CustomerPage");
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
            var product = _reviewManagement.GetProductName(productId);

            //var product = _productService.getProductDetails(productId);
            if (product == null)
            {
                TempData["Error"] = "Product not found.";
                return RedirectToAction("GetAllProducts");
            }

            var reviews = _reviewManagement.ViewReviewsByProduct(productId);
            ViewBag.ProductName = product; 

            //ViewBag.ProductName = product.GetProductDetails()["ProductName"];
            ViewBag.ProductId = productId;

            return View("~/Views/Review/ProductReviews.cshtml", reviews);
        }



        


    }
}