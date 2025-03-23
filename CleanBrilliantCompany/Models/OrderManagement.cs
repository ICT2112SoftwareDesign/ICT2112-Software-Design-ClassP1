using System;
using System.Collections.Generic;
using System.Linq;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models
{
    public class OrderManagement : IOrder
    {
    
        private readonly IOrderDatabase _orderDatabase;
        private readonly ICartManagement _cartManagement;
        // private readonly ISubmitRefund _submitRefund;

        // Place ISubmitRefund submitRefund in params
        public OrderManagement(IOrderDatabase orderDatabase, ICartManagement cartManagement)
        {
            _orderDatabase = orderDatabase;
            _cartManagement = cartManagement;
            // _submitRefund = submitRefund;
        }

        public int createOrder(
            int customerId,
            string deliveryAddress,
            string serviceType,
            string shippingType,
            string shippingAgent,
            Dictionary<int, int> cart)
        {
            try
            {
                // Fetch product details for the cart
                var products = _cartManagement.getCartProductDetails(cart);
                if (products == null || products.Count == 0)
                {
                    throw new Exception("Failed to fetch product details for the cart.");
                }

                // Calculate the cart total
                decimal cartTotal = _cartManagement.calculateCartTotal(cart, products);

                // Calculate the shipping fee
                decimal shippingFee = calculateShippingFee(serviceType);

                // Serialize the shipping details into JSON
                var shippingDetails = new
                {
                    ShippingAgent = shippingAgent,
                    ShippingMethod = shippingType,
                    ServiceType = serviceType,
                    ShippingFee = shippingFee.ToString("F2") // Include the shipping fee here
                };
                string orderShippingJson = System.Text.Json.JsonSerializer.Serialize(shippingDetails);

                // Create the order object
                var order = new OrderRDM
                {
                    CustomerID = customerId,
                    OrderAddress = deliveryAddress,
                    OrderProducts = cart, // Store the cart dictionary directly
                    OrderShipping = orderShippingJson,
                    OrderItems = cart.Count,
                    OrderDate = DateTime.Now,
                    Status = "Pending",
                    OrderTotal = cartTotal + shippingFee
                };

                // Save the order to the database
                return _orderDatabase.insertOrder(order);
            }
            catch (Exception ex)
            {
                // Log the error and rethrow or handle it
                throw new Exception($"Failed to create order: {ex.Message}");
            }
        }

        public decimal calculateShippingFee(string serviceType)
        {
            var serviceCosts = new Dictionary<string, decimal>
            {
                { "1 Day", 10.00m },
                { "3 Days", 5.00m },
                { "7 Days", 0.00m }
            };

            if (!serviceCosts.ContainsKey(serviceType))
            {
                throw new Exception("Invalid service type selected.");
            }

            return serviceCosts[serviceType];
        }

        // Implementation of getOrderDetails
        public OrderRDM getOrderDetails(int orderId)
        {
            return _orderDatabase.getOrderById(orderId);
        }

        // Implementation of getOrderHistory
        public List<OrderRDM> getOrderHistory(int customerId)
        {
            return _orderDatabase.getOrdersByCustomerId(customerId);
        }

        // Implementation of cancelOrder
        public bool cancelOrder(int orderId)
        {
            var order = _orderDatabase.getOrderById(orderId);
            if (order == null || order.Status == "Cancelled")
            {
                return false;
            }

            order.Status = "Cancelled";
            return _orderDatabase.updateOrder(order);
        }

        // Implementation of updateOrderStatus
        public bool updateOrderStatus(int orderId, string status)
        {
            var order = _orderDatabase.getOrderById(orderId);
            if (order == null)
            {
                return false;
            }

            order.Status = status;
            return _orderDatabase.updateOrder(order);
        }

        public bool cancelOrder(int orderId, int customerId)
        {
            var order = _orderDatabase.getOrderById(orderId);
            if (order == null || order.CustomerID != customerId || order.Status != "Pending")
            {
                return false; // Cannot cancel the order
            }

            order.Status = "Cancelled";
            return _orderDatabase.updateOrder(order);
        }

        public bool requestRefund(int orderId, int customerId, string refundReason, string refundImage, string refundVideo)
        {
            var order = _orderDatabase.getOrderById(orderId);
            if (order == null || order.CustomerID != customerId || order.Status != "Completed")
            {
                return false; // Cannot request a refund
            }
            Console.WriteLine($"{refundReason}, {refundImage}, {refundVideo}, {customerId}");
            order.Status = "RefundRequested";

            // I cant do this without concrete implementation of submit refund yet
            // _submitRefund.submitRefund(orderId, customerId, order.Status, refundReason, refundImage, refundVideo);
            return _orderDatabase.updateOrder(order);
        }






    }
}