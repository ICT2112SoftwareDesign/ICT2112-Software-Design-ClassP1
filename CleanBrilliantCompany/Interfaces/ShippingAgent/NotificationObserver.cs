namespace CleanBrilliantCompany.Interfaces
{
    public interface NotificationObserver
    {
        void onOrderCompleted(int orderId, string orderStatus);
        void onOrderShipped(int orderId, string orderStatus);
        void onOrderRefundRequested(int orderId, string orderStatus);
        void onOrderCancelled(int orderId, string orderStatus);
        void onOrderPending(int orderId, string orderStatus);
    }
}