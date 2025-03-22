using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Models
{
    public class ReviewManagement 
    {    
        private ReviewRDM reviewRDM; 
        private IReviewDatabase reviewDatabase;
        private IProduct productService;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IReviewQueryObserver _observer;

        
        


        public ReviewManagement(IReviewDatabase reviewDatabase, IProduct productService, IHttpContextAccessor httpContextAccessor, IReviewQueryObserver observer)
        {
            this.reviewDatabase = reviewDatabase;
            this.productService = productService;
            this._httpContextAccessor = httpContextAccessor;
            this._observer = observer;
            this.reviewRDM = new ReviewRDM();
        }

        //get customer ID directly from session 
        private int? GetLoggedInCustomerId() 
        { 
            return _httpContextAccessor.HttpContext.Session.GetInt32("LoggedInUserId"); 
        }

        //methods that use the session directly 
        public bool WriteReview(string reviewText, int rating, int productId)
        { 
            // Get customer ID from session
            int? customerId = GetLoggedInCustomerId();
            if (customerId == null)
                return false;
            
             // Validate rating is between 1-5
            if (rating < 1 || rating > 5)
                return false;
           
             // Validate product exists
            var product = productService.getProductDetails(productId);
            if (product == null)
                return false;

             // Validate review text
            if (string.IsNullOrWhiteSpace(reviewText))
                return false;


           


            bool success = reviewRDM.CreateReview(customerId.Value, reviewText, rating, productId); // 0 as placeholder


             if (success)
            {
                success = reviewDatabase.addReview(customerId.Value, reviewText, rating, productId);
                if (success)
                    _observer.OnReviewSubmitted(customerId.Value, productId, rating);
                else
                    _observer.OnReviewQueryFailed(customerId.Value, "Database insert failed.");
            }

            return success;


        }

        public bool EditReview (int reviewId, string reviewText, int rating)
        { 
            int? customerId = GetLoggedInCustomerId();
            if (customerId == null || rating < 1 || rating > 5 || string.IsNullOrWhiteSpace(reviewText))
                return false;

            //if (!VerifyReviewOwnership(reviewId, customerId.Value))
            //    return false;

            reviewRDM.SetReview(reviewText);
            reviewRDM.SetRating(rating);

            bool success = reviewDatabase.updateReview(customerId.Value, reviewText, rating, reviewId);

            if (success)
                _observer.OnReviewUpdated(reviewId, customerId.Value, rating);
            else
                _observer.OnReviewQueryFailed(customerId.Value, "Update failed in database.");


            return success;
        }

        //delete a reivew 
        public bool DeleteReview(int reviewId)
        { 
             int? customerId = GetLoggedInCustomerId();
            if (customerId == null)
                return false;

            bool success = reviewDatabase.deleteReview(customerId.Value, reviewId);
            if (success)
                _observer.OnReviewDeleted(reviewId, customerId.Value);
            else
                _observer.OnReviewQueryFailed(customerId.Value, "Failed to delete review from database.");

            return success;
        }


        //list all reviews 
         public List<ReviewRDM> ViewReviews()
        {
            return ConvertToReviewModel(reviewDatabase.GetAllReviews());
        }

        // public void NotifyDBReviewQueryStatus()
        //{
            // Hook for observer pattern – logging, monitoring, etc.
        //}

         private bool VerifyReviewOwnership(int reviewId, int customerId)
        {
            // TODO: Check if review with reviewId belongs to customerId in DB
            return true; // placeholder
        }

        private List<ReviewRDM> ConvertToReviewModel(List<ReviewRDM> rawReviews)
        {
            return rawReviews; // can map if needed
        }



    }

    
}