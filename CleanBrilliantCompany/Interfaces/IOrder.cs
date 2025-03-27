using System.Collections.Generic;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IOrder
    {
        OrderRDM getOrderDetails(int orderId);
        List<OrderRDM> getAllOrders();
        List<int> getOrderItemIds(int orderId);
        
    }
}