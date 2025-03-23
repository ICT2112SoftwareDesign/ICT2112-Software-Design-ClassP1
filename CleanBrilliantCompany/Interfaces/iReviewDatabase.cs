using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IReviewDatabase
    {
        bool addReview(int customerId, string reviewText, int rating, int productId);
        bool updateReview(int customerId, string reviewText, int rating, int reviewId);
        bool deleteReview(int customerId, int reviewId);

        List<ReviewRDM> GetAllReviews(); 
    }
}