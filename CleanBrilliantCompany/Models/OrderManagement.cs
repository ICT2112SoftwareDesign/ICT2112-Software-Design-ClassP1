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

        public OrderManagement(IOrderDatabase orderDatabase, ICartManagement cartManagement)
        {
            _orderDatabase = orderDatabase;
            _cartManagement = cartManagement;
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
                    ShippingFee = shippingFee.ToString("F2")
                };
                string orderShippingJson = System.Text.Json.JsonSerializer.Serialize(shippingDetails);

                // Create the order object
                var order = new OrderRDM(
                    orderID: 0, // Will be set by the database
                    customerID: customerId,
                    orderAddress: deliveryAddress,
                    orderProducts: cart,
                    orderShipping: orderShippingJson,
                    orderItems: cart.Keys.ToList(), // Use product IDs as serial numbers
                    orderDate: DateTime.Now,
                    status: "Pending",
                    orderTotal: cartTotal + shippingFee
                );

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

        public OrderRDM getOrderDetails(int orderId)
        {
            return _orderDatabase.getOrderById(orderId);
        }

        public List<OrderRDM> getOrderHistory(int customerId)
        {
            return _orderDatabase.getOrdersByCustomerId(customerId);
        }

        public bool cancelOrder(int orderId)
        {
            var order = _orderDatabase.getOrderById(orderId);
            if (order == null || order.GetStatus() == "Cancelled")
            {
                return false;
            }

            order.SetStatus("Cancelled");
            return _orderDatabase.updateOrder(order);
        }

        public bool updateOrderStatus(int orderId, string status)
        {
            var order = _orderDatabase.getOrderById(orderId);
            if (order == null)
            {
                return false;
            }

            order.SetStatus(status);
            return _orderDatabase.updateOrder(order);
        }

        public bool cancelOrder(int orderId, int customerId)
        {
            var order = _orderDatabase.getOrderById(orderId);
            if (order == null || order.GetCustomerID() != customerId || order.GetStatus() != "Pending")
            {
                return false; // Cannot cancel the order
            }

            order.SetStatus("Cancelled");
            return _orderDatabase.updateOrder(order);
        }

        public bool requestRefund(int orderId, int customerId, string refundReason)
        {
            var order = _orderDatabase.getOrderById(orderId);
            if (order == null || order.GetCustomerID() != customerId || order.GetStatus() != "Completed")
            {
                return false; // Cannot request a refund
            }

            Console.WriteLine($"{orderId}, {refundReason} {order.GetOrderTotal()}, {order.GetOrderProducts()}, {customerId}");
            order.SetStatus("RefundRequested");

            // I can't do this without a concrete implementation of submitRefund yet
            // _submitRefund.submitRefund(orderId, refundReason, order.GetOrderTotal(), order.GetOrderProducts());
            return _orderDatabase.updateOrder(order);
        }
    }
}