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

         public OrderRDM getOrderDetails(int orderId)
        {
            try
            {
                return _orderDatabase.getOrderById(orderId);
            }
            catch (Exception ex)
            {
                // Log error
                throw new Exception($"Failed to fetch order details: {ex.Message}");
            }
        }

        public List<OrderRDM> getOrderHistory(int customerId)
        {
            try
            {
                return _orderDatabase.getOrdersByCustomerId(customerId);
            }
            catch (Exception ex)
            {
                // Log error
                throw new Exception($"Failed to fetch order history: {ex.Message}");
            }
        }

        public List<OrderRDM> getAllOrders()
        {
            try
            {
                var orders = _order.getAllOrders();

                // Add validation
                if (orders == null)
                {
                    throw new Exception("No orders found");
                }

                // Ensure we're getting OrderRDM objects
                if (orders.Count > 0 && !(orders[0] is OrderRDM))
                {
                    throw new Exception("Invalid order data format");
                }

                return orders;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to fetch all orders: {ex.Message}");
            }
        }
        public bool cancelOrder(int orderId)
        {
            try
            {
                var order = _orderDatabase.getOrderById(orderId);
                if (order == null || order.RetrieveStatus() == "Cancelled")
                {
                    return false;
                }

                order.UpdateStatus("Cancelled");
                return _orderDatabase.updateOrder(order);
            }
            catch (Exception ex)
            {
                // Log error
                throw new Exception($"Failed to cancel order: {ex.Message}");
            }
        }

        public bool updateOrderStatus(int orderId, string status)
        {
            try
            {
                // Directly update the status in database
                return _order.updateOrderStatus(orderId, status);
            }
            catch (Exception ex)
            {
                // Log error
                Console.WriteLine($"Error updating order status: {ex.Message}");
                return false;
            }
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

        public void onOrderPending(int orderId, string orderStatus) 
        {
            foreach (var observer in _observers)
            {
                observer.onOrderPending(orderId, orderStatus);
            }
        }
    }
}