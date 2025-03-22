using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Management;

namespace CleanBrilliantCompany.Controllers
{
    public class OrderFulfilmentController
    {
        private readonly IOrder _orderFulfilmentManagement;

        public OrderFulfilmentController(IOrder orderFulfilmentManagement)
        {
            _orderFulfilmentManagement = orderFulfilmentManagement;
        }

        public OrderRDM fetchOrderDetails(int orderId)
        {
            return _orderFulfilmentManagement.getOrderDetails(orderId);
        }

        public bool updateOrderStatus(int orderId, string status)
        {
            return _orderFulfilmentManagement.updateOrderStatus(orderId, status);
        }

        // Additional methods can be added here as needed
    }
}