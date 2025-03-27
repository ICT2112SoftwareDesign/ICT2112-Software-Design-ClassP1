using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using System;
using System.Collections.Generic;

namespace CleanBrilliantCompany.Models
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
                var orders = _orderDatabase.getAllOrders();

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
                if (order == null || order.GetStatus() == "Cancelled")
                {
                    return false;
                }

                order.SetStatus("Cancelled");
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
                return _orderDatabase.updateOrderStatus(orderId, status);
            }
            catch (Exception ex)
            {
                // Log error
                Console.WriteLine($"Error updating order status: {ex.Message}");
                return false;
            }
        }
    }
}