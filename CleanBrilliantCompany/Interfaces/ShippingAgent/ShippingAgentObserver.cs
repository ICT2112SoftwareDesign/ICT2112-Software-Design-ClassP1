using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Interfaces
{
    public class ShippingAgentObserver : IShippingNotification, NotificationObserver
    {
        public void onOrderCompleted(int orderId, string orderStatus)
        {
            Console.WriteLine($"ShippingAgentObserver: Order {orderId} marked as {orderStatus}.");
        }

        public void onOrderShipped(int orderId, string orderStatus)
        {
            Console.WriteLine($"ShippingAgentObserver: Order {orderId} marked as {orderStatus}.");
        }

        public void onOrderRefundRequested(int orderId, string orderStatus)
        {
            Console.WriteLine($"ShippingAgentObserver: Order {orderId} marked as {orderStatus}.");
        }

        public void onOrderCancelled(int orderId, string orderStatus)
        {
            Console.WriteLine($"ShippingAgentObserver: Order {orderId} marked as {orderStatus}.");
        }

        public void onOrderPending(int orderId, string orderStatus)
        {
            Console.WriteLine($"ShippingAgentObserver: Order {orderId} marked as {orderStatus}.");
        }
    }
}