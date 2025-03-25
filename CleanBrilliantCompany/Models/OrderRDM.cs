using System;
using System.Collections.Generic;

namespace CleanBrilliantCompany.Models
{
    public class OrderRDM
    {
        // Private fields
        private int _orderID;
        private int _customerID;
        private string _orderAddress;
        private Dictionary<int, int> _orderProducts; // Product ID -> Quantity
        private string _orderShipping;
        private List<int> _orderItems; // List of serial numbers for products
        private DateTime _orderDate;
        private string _status;
        private decimal _orderTotal;
        private Dictionary<int, Dictionary<string, object>> _orderProductsDetails;

        // Constructor to initialize the fields
        public OrderRDM(
            int orderID,
            int customerID,
            string orderAddress,
            Dictionary<int, int> orderProducts,
            string orderShipping,
            List<int> orderItems,
            DateTime orderDate,
            string status,
            decimal orderTotal)
        {
            _orderID = orderID;
            _customerID = customerID;
            _orderAddress = orderAddress;
            _orderProducts = orderProducts;
            _orderShipping = orderShipping;
            _orderItems = orderItems ?? new List<int>(); // Ensure it's not null
            _orderDate = orderDate;
            _status = status;
            _orderTotal = orderTotal;
            _orderProductsDetails = new Dictionary<int, Dictionary<string, object>>();
        }

        // Private getters and setters
        private int GetOrderID() => _orderID;
        private void SetOrderID(int orderID) => _orderID = orderID;

        private int GetCustomerID() => _customerID;
        private void SetCustomerID(int customerID) => _customerID = customerID;

        private string GetOrderAddress() => _orderAddress;
        private void SetOrderAddress(string orderAddress) => _orderAddress = orderAddress;

        private Dictionary<int, int> GetOrderProducts() => _orderProducts;
        private void SetOrderProducts(Dictionary<int, int> orderProducts) => _orderProducts = orderProducts;

        private string GetOrderShipping() => _orderShipping;
        private void SetOrderShipping(string orderShipping) => _orderShipping = orderShipping;

        private List<int> GetOrderItems() => _orderItems;
        private void SetOrderItems(List<int> orderItems) => _orderItems = orderItems ?? new List<int>();

        private DateTime GetOrderDate() => _orderDate;
        private void SetOrderDate(DateTime orderDate) => _orderDate = orderDate;

        private string GetStatus() => _status;
        private void SetStatus(string status) => _status = status;

        private decimal GetOrderTotal() => _orderTotal;
        private void SetOrderTotal(decimal orderTotal) => _orderTotal = orderTotal;

        private Dictionary<int, Dictionary<string, object>> GetOrderProductsDetails() => _orderProductsDetails;
        private void SetOrderProductsDetails(Dictionary<int, Dictionary<string, object>> orderProductsDetails) => _orderProductsDetails = orderProductsDetails;

        // Public methods to expose necessary functionality

        public int RetrieveOrderID() => GetOrderID();
        public int RetrieveCustomerID() => GetCustomerID();
        public string RetrieveOrderAddress() => GetOrderAddress();
        public Dictionary<int, int> RetrieveOrderProducts() => GetOrderProducts();
        public string RetrieveOrderShipping() => GetOrderShipping();
        public List<int> RetrieveOrderItems() => GetOrderItems();
        public DateTime RetrieveOrderDate() => GetOrderDate();
        public string RetrieveStatus() => GetStatus();
        public decimal RetrieveOrderTotal() => GetOrderTotal();
        public Dictionary<int, Dictionary<string, object>> RetrieveOrderProductsDetails() => GetOrderProductsDetails();

        public void UpdateOrderAddress(string newAddress) => SetOrderAddress(newAddress);
        public void UpdateOrderProducts(Dictionary<int, int> newProducts) => SetOrderProducts(newProducts);
        public void UpdateOrderShipping(string newShipping) => SetOrderShipping(newShipping);
        public void UpdateOrderItems(List<int> newItems) => SetOrderItems(newItems);
        public void UpdateOrderDate(DateTime newDate) => SetOrderDate(newDate);
        public void UpdateStatus(string newStatus) => SetStatus(newStatus);
        public void UpdateOrderTotal(decimal newTotal) => SetOrderTotal(newTotal);
        public void UpdateOrderProductsDetails(Dictionary<int, Dictionary<string, object>> newDetails) => SetOrderProductsDetails(newDetails);
    }
}