using System;
using System.Collections.Generic;
using System.Linq;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models
{
    public class OrderManagement
    {
    
        private readonly IOrderDatabase _orderDatabase;
        private readonly ICartManagement _cartManagement;

        public OrderManagement( IOrderDatabase orderDatabase, ICartManagement cartManagement)
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
                    ServiceType = serviceType
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
                return _orderDatabase.InsertOrder(order);
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
    }
}