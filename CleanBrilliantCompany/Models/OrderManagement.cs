using System;
using System.Collections.Generic;
using System.Linq;

namespace CleanBrilliantCompany.Models
{
    public class OrderManagement
    {
        private readonly IOrderDatabase _orderDatabase;

        public OrderManagement(IOrderDatabase orderDatabase)
        {
            _orderDatabase = orderDatabase;
        }

        // Method to create an order from the cart
        public int createOrderFromCart(
            int customerId,
            string deliveryAddress,
            string serviceType,
            string shippingType,
            string shippingAgent,
            Dictionary<int, int> cart)
        {
            // Calculate the total price
            decimal cartTotal = 0;
            var products = new Dictionary<int, Dictionary<string, object>>();
            foreach (var item in cart)
            {
                // Simulate fetching product details (replace with actual product service)
                var productDetails = new Dictionary<string, object>
                {
                    { "ProductName", $"Product {item.Key}" },
                    { "CostPrice", 10.00m } // Example price
                };
                products[item.Key] = productDetails;
                cartTotal += Convert.ToDecimal(productDetails["CostPrice"]) * item.Value;
            }

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
                OrderProducts = string.Join(", ", products.Select(p => $"{p.Value["ProductName"]} x {cart[p.Key]}")),
                OrderShipping = orderShippingJson,
                OrderItems = cart.Count,
                OrderDate = DateTime.Now,
                Status = "Pending",
                OrderTotal = cartTotal
            };

            // Save the order to the database
            return _orderDatabase.InsertOrder(order);
        }
    }
}