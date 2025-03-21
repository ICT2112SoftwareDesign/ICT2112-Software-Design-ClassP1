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
        
        // Public Method to Create an Order
        public void CreateOrder(
            int orderId,
            int customerId,
            DateTime orderDate,
            string orderAddress,
            Dictionary<int, int> orderProducts,
            Dictionary<string, string> orderShipping,
            decimal orderTotal)
        {
            this.orderId = orderId;
            this.customerId = customerId;
            this.orderDate = orderDate;
            this.orderAddress = orderAddress;
            this.orderProducts = orderProducts;
            this.orderShipping = orderShipping;
            this.status = OrderStatus.Pending; // Default status
            this.orderTotal = orderTotal;
        }

        // Public Method to Update the Order Status
        public void UpdateStatus(OrderStatus newStatus)
        {
            this.status = newStatus;
        }

        // Public Method to Calculate the Total Cost of the Order
        public decimal CalculateTotal(Dictionary<int, decimal> productPrices)
        {
            if (orderProducts == null || orderProducts.Count == 0)
            {
                throw new InvalidOperationException("OrderProducts is empty. Cannot calculate total.");
            }

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

        // Public Method to Get Order Details
        public Dictionary<string, object> GetOrderDetails()
        {
            return new Dictionary<string, object>
            {
                { "OrderId", orderId },
                { "CustomerId", customerId },
                { "OrderDate", orderDate },
                { "OrderAddress", orderAddress },
                { "OrderProducts", orderProducts },
                { "OrderShipping", orderShipping },
                { "OrderStatus", status },
                { "OrderTotal", orderTotal }
            };
        }
    }

    // Assuming Item class is defined elsewhere in your project
    public class Item
    {
        // Item properties and methods
    }
}