using System.Collections.Generic;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IOrderDatabase
    {
        // Add a new order to the database and return the generated order ID
        int createOrder(OrderRDM order);

        // Retrieve a specific order by its ID
        OrderRDM RetrieveOrder(int orderId);

        // Retrieve all orders for a specific customer
        List<OrderRDM> RetrieveOrders(int customerId);

        // Update an existing order in the database
        bool UpdateOrder(OrderRDM order);

        // Delete an order by its ID
        bool DeleteOrder(int orderId);
    }
}