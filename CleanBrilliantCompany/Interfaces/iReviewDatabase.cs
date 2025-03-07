namespace CleanBrilliantCompany.Interfaces
{
    public interface iReviewDatabase
    {
        bool addReview(int customerId, string review, int rating);
        bool updateReview(int customerId, string review, int rating);
        bool deleteReview(int customerId, int reviewId);
    }
}