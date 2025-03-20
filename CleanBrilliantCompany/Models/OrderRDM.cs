using System;
using System.Collections.Generic;

namespace CleanBrilliantCompany.Models
{
    public class OrderRDM
    {
        // Properties
        private int orderId;
        private int customerId;
        private DateTime orderDate;
        private string orderAddress;
        private Dictionary<int, int> orderProducts; // ProductID, Quantity
        private Dictionary<string, string> orderShipping; // Shipping details
        private List<Item> orderItems;
        private OrderStatus status;
        private decimal orderTotal;

        // Enum for Order Status
        public enum OrderStatus
        {
            Pending,
            Processing,
            Shipped,
            Delivered,
            Cancelled
        }

        // Private Getters and Setters
        private int getOrderId() => orderId;
        private void setOrderId(int orderId) => this.orderId = orderId;

        private int getCustomerId() => customerId;
        private void setCustomerId(int customerId) => this.customerId = customerId;

        private DateTime getOrderDate() => orderDate;
        private void setOrderDate(DateTime orderDate) => this.orderDate = orderDate;

        private string getOrderAddress() => orderAddress;
        private void setOrderAddress(string orderAddress) => this.orderAddress = orderAddress;

        private Dictionary<int, int> getOrderProducts() => orderProducts;
        private void setOrderProducts(Dictionary<int, int> orderProducts) => this.orderProducts = orderProducts;

        private Dictionary<string, string> getOrderShipping() => orderShipping;
        private void setOrderShipping(Dictionary<string, string> orderShipping) => this.orderShipping = orderShipping;

        private List<Item> getOrderItems() => orderItems;
        private void setOrderItems(List<Item> orderItems) => this.orderItems = orderItems;

        private OrderStatus getStatus() => status;
        private void setStatus(OrderStatus status) => this.status = status;

        private decimal getOrderTotal() => orderTotal;
        private void setOrderTotal(decimal orderTotal) => this.orderTotal = orderTotal;
        
        // Method to create an order
        public bool createOrder(int orderId, int customerId, DateTime orderDate, string orderAddress, Dictionary<int, int> orderProducts, Dictionary<string, string> orderShipping, decimal orderTotal)
        {
            setOrderId(orderId);
            setCustomerId(customerId);
            setOrderDate(orderDate);
            setOrderAddress(orderAddress);
            setOrderProducts(orderProducts);
            setOrderShipping(orderShipping);
            setStatus(OrderStatus.Pending);
            setOrderTotal(orderTotal);

            // Additional logic to save the order to the database can be added here

            return true; // Return true if the order is created successfully
        }

        // Method to calculate the total cost of the order
        public decimal calculateTotal(Dictionary<int, decimal> productPrices)
        {
            decimal total = 0;
            foreach (var item in orderProducts)
            {
                if (productPrices.ContainsKey(item.Key))
                {
                    total += productPrices[item.Key] * item.Value;
                }
            }
            return total;
        }
    }

    // Assuming Item class is defined elsewhere in your project
    public class Item
    {
        // Item properties and methods
    }
}