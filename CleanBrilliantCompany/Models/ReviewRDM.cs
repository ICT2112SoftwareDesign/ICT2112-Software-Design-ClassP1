using System;

namespace CleanBrilliantCompany.Models
{
    public class ReviewRDM
    {
            private int _reviewId;
            private int _customerId;
            private string _reviewText;
            private int _rating;
            private int _productId;

            //constructor 
            public ReviewRDM (int reviewId, int customerId, string reviewText, int rating, int productId)
            { 
                _reviewId = reviewId; 
                _customerId = customerId; 
                _reviewText = reviewText; 
                _rating = rating; 
                _productId = productId; 

            }
            //private getters/setters 
            private int GetReviewId() => _reviewId; 
            private void SetReviewId(int id) => _reviewId = id;

            private int GetCustomerId() => _customerId;
            private void SetCustomerId(int id) => _customerId = id;

            private string GetReviewText() => _reviewText;
            private void SetReviewText(string text) => _reviewText = text;

            private int GetRating() => _rating;
            private void SetRating(int rating) => _rating = rating;

            private int GetProductId() => _productId;
            private void SetProductId(int id) => _productId = id;

            // Public retrieval methods
            public int RetrieveReviewId() => GetReviewId();
            public int RetrieveCustomerId() => GetCustomerId();
            public string RetrieveReviewText() => GetReviewText();
            public int RetrieveRating() => GetRating();
            public int RetrieveProductId() => GetProductId();

             // Public update methods
            public void UpdateReviewText(string newText) => SetReviewText(newText);
            public void UpdateRating(int newRating) => SetRating(newRating);
            public void UpdateReview(string newText, int newRating)
            {
                SetReviewText(newText);
                SetRating(newRating);
            }

            // Getters and Setters
        //public int GetCustomerId()
        //{
         //   return customerId;
        //}
        /*
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
        }*/
    }
}