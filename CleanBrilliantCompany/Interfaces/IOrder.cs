using System.Collections.Generic;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IOrder
    {
        OrderRDM getOrderDetails(int orderId);
        List<OrderRDM> getOrderHistory(int customerId);
        bool cancelOrder(int orderId);
        bool updateOrderStatus(int orderId, string status);
        List<OrderRDM> getAllOrders();
        List<int> getOrderItemIds(int orderId);
        
    }
}