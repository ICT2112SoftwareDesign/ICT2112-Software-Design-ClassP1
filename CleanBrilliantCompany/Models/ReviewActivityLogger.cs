using System;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Observers
{
    public class ReviewActivityLogger : IReviewQueryObserver
    {
        public void OnReviewSubmitted(int customerId, int productId, int rating)
        {
            Console.WriteLine($"Review submitted by Customer {customerId} for Product {productId} with rating {rating}.");
        }

        public void OnReviewUpdated(int reviewId, int customerId, int rating)
        {
            Console.WriteLine($" Review {reviewId} updated by Customer {customerId} with new rating {rating}.");
        }

        public void OnReviewDeleted(int reviewId, int customerId)
        {
            Console.WriteLine($" Review {reviewId} deleted by Customer {customerId}.");
        }

        public void OnReviewQueryFailed(int customerId, string reason)
        {
            Console.WriteLine($" Review operation failed for Customer {customerId}. Reason: {reason}");
        }
    }
}
