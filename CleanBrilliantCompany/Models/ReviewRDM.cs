using System;

namespace CleanBrilliantCompany.Models
{
    public class ReviewRDM
    {
            private int reviewId;
            private int customerId;
            private string reviewText;
            private int rating;
            private int productId;

            // Getters and Setters
        public int GetCustomerId()
        {
            return customerId;
        }

        public void SetCustomerId(int customerId)
        {
            this.customerId = customerId;
        }

        public int GetReviewId()
        {
            return reviewId;
        }

        public void SetReviewId(int reviewId)
        {
            this.reviewId = reviewId;
        }

        public string GetReview()
        {
            return reviewText;
        }

        public void SetReview(string reviewText)
        {
            this.reviewText = reviewText;
        }

        public int GetRating()
        {
            return rating;
        }

        public void SetRating(int rating)
        {
            this.rating = rating;
        }

        public int GetProductId()
        {
            return productId;
        }

        public void SetProductId(int productId)
        {
            this.productId = productId;
        }
    
        public bool CreateReview(int customerId, string reviewText, int rating, int productId)
        {
            this.customerId = customerId;
            this.reviewText = reviewText;
            this.rating = rating;
            this.productId = productId;

            // assuming successful creation
            return true;
        }
    }
}