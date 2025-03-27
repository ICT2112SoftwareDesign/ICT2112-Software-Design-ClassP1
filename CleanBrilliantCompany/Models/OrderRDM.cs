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
        private double _orderWeight;  // New field for total weight

        // Constructor to initialize all fields, including orderWeight.
        public OrderRDM(
            int orderID,
            int customerID,
            string orderAddress,
            Dictionary<int, int> orderProducts,
            string orderShipping,
            List<int> orderItems,
            DateTime orderDate,
            string status,
            decimal orderTotal,
            double orderWeight)
        {
            _orderID = orderID;
            _customerID = customerID;
            _orderAddress = orderAddress;
            _orderProducts = orderProducts;
            _orderShipping = orderShipping;
            _orderItems = orderItems ?? new List<int>();
            _orderDate = orderDate;
            _status = status;
            _orderTotal = orderTotal;
            _orderProductsDetails = new Dictionary<int, Dictionary<string, object>>();
            _orderWeight = orderWeight;
        }

        // Public methods to access and modify the fields.
        public int GetOrderID() => _orderID;
        public void SetOrderID(int orderID) => _orderID = orderID;

        public int GetCustomerID() => _customerID;
        public void SetCustomerID(int customerID) => _customerID = customerID;

        public string GetOrderAddress() => _orderAddress;
        public void SetOrderAddress(string orderAddress) => _orderAddress = orderAddress;

        public Dictionary<int, int> GetOrderProducts() => _orderProducts;
        public void SetOrderProducts(Dictionary<int, int> orderProducts) => _orderProducts = orderProducts;

        public string GetOrderShipping() => _orderShipping;
        public void SetOrderShipping(string orderShipping) => _orderShipping = orderShipping;

        /// Gets the list of serial numbers for the products in the order.
        public List<int> GetOrderItems() => _orderItems;
        /// Sets the list of serial numbers for the products in the order.
        public void SetOrderItems(List<int> orderItems) => _orderItems = orderItems ?? new List<int>();

        public DateTime GetOrderDate() => _orderDate;
        public void SetOrderDate(DateTime orderDate) => _orderDate = orderDate;

        public string GetStatus() => _status;
        public void SetStatus(string status) => _status = status;

        public decimal GetOrderTotal() => _orderTotal;
        public void SetOrderTotal(decimal orderTotal) => _orderTotal = orderTotal;

        public Dictionary<int, Dictionary<string, object>> GetOrderProductsDetails() => _orderProductsDetails;
        public void SetOrderProductsDetails(Dictionary<int, Dictionary<string, object>> orderProductsDetails) => _orderProductsDetails = orderProductsDetails;

        // New public property for OrderWeight.
        public double OrderWeight
        {
            get { return _orderWeight; }
            set { _orderWeight = value; }
        }
    }
}
