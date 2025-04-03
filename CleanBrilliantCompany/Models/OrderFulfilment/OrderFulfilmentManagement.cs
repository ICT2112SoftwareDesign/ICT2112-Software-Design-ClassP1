using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Observers;

namespace CleanBrilliantCompany.Models
{
    public class OrderFulfilmentManagement : IShippingNotification
    {
        private readonly List<IShippingNotification> _observers = new List<IShippingNotification>();

        public OrderFulfilmentManagement(IOrder order, IOrderDatabase orderDatabase)
        {
            _order = order;
            _orderDatabase = orderDatabase;

            // Add observers
            _observers.Add(new ShippingAgentObserver());
            _observers.Add(new NotificationObserver());
        }

        public void onOrderCompleted(int orderId, string orderStatus)
        {
            foreach (var observer in _observers)
            {
                observer.onOrderCompleted(orderId, orderStatus);
            }
        }

        public void onOrderShipped(int orderId, string orderStatus)
        {
            foreach (var observer in _observers)
            {
                observer.onOrderShipped(orderId, orderStatus);
            }
        }

        public void onOrderRefundRequested(int orderId, string orderStatus)
        {
            foreach (var observer in _observers)
            {
                observer.onOrderRefundRequested(orderId, orderStatus);
            }
        }

        public void onOrderCancelled(int orderId, string orderStatus) 
        {
            foreach (var observer in _observers)
            {
                observer.onOrderCancelled(orderId, orderStatus);
            }
        }

        public void onOrderPending(int orderId, string orderStatus) /
        {
            foreach (var observer in _observers)
            {
                observer.onOrderPending(orderId, orderStatus);
            }
        }
    }
}