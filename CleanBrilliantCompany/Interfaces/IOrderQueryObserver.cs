namespace CleanBrilliantCompany.Interfaces
{
    public interface IOrderQueryObserver
    {
        void onOrderCreated(int orderId, int customerId, decimal orderTotal);
        void onOrderUpdated(int orderId, int customerId, string status);
        void onOrderCancelled(int orderId, int customerId);
    }
}