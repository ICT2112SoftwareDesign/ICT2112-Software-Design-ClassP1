namespace CleanBrilliantCompany.Interfaces
{
    public interface IReviewQueryObserver
    {
        void OnReviewSubmitted(int customerId, int productId, int rating);
        void OnReviewUpdated(int reviewId, int customerId, int rating);
        void OnReviewDeleted(int reviewId, int customerId);
        void OnReviewQueryFailed(int customerId, string reason);
    }
}