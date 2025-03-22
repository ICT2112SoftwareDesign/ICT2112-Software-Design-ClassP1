using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Management
{
    public class OrderFulfilmentManagement : IOrder
    {
        private readonly IOrderDatabase _orderDatabase;

        public OrderFulfilmentManagement(IOrderDatabase orderDatabase)
        {
            _orderDatabase = orderDatabase;
        }

        public OrderRDM getOrderDetails(int orderId)
        {
            return _orderDatabase.getOrderById(orderId);
        }

        public List<OrderRDM> getOrderHistory(int customerId)
        {
            return _orderDatabase.getOrdersByCustomerId(customerId);
        }
        public List<OrderRDM> getAllOrders()
        {
            return _orderDatabase.getAllOrders(); // Use the new method
        }

        public bool cancelOrder(int orderId)
        {
            var order = _orderDatabase.getOrderById(orderId);
            if (order != null)
            {
                order.Status = "Cancelled";
                return _orderDatabase.updateOrder(order);
            }
            return false;
        }

        public bool updateOrderStatus(int orderId, string status)
        {
            var order = _orderDatabase.getOrderById(orderId);
            if (order != null)
            {
                order.Status = status;
                return _orderDatabase.updateOrder(order);
            }
            return false;
        }

        // Additional methods can be added here as needed
    }
}