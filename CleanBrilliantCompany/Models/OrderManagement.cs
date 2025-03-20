using System;
using System.Collections.Generic;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models
{
    public class OrderManagement
    {
        private readonly IOrderDatabase _orderDatabase;

        private readonly IProduct _product;
        private readonly IShippingAgents _shippingAgents;

        public OrderManagement(IOrderDatabase orderDatabase,IShippingAgents shippingAgents)
        {
            _orderDatabase = orderDatabase;
            _shippingAgents = shippingAgents;
            
        }

       //public int CreateOrder(int customerId, string orderAddress, string deliveryTime, Service shippingType, string shippingAgent, Dictionary<int, int> orderProducts, decimal orderTotal)
       // {
            // Create a new order
        //    var orderId = _orderDatabase.createOrder(customerId, orderAddress, deliveryTime, shippingType, shippingAgent, orderProducts, orderTotal);

            // Notify the DB of the order query status
        //    notifyDBOrderQueryStatus();

        //    return orderId;
       // }

        public bool makePayment()
        {
            // Implementation for making a payment
            return true;
        }

       // public List<OrderRDM> getOrderHistory(int customerId)
        //{
            // Retrieve orders from the database based on customerId
        //    return _orderDatabase.retrieveOrders(customerId);
       // }

       // public string getOrderStatus(int orderId)
       // {
        //  return orderId;
       // }

        public bool cancelOrder(int orderId)
        {
            // Retrieve the order from the database
            return false;
        }

        public bool requestReturn(int orderId)
        {
            // Retrieve the order from the database 
            return false;
        }

        public void changeNotificationSetting()
        {
            // Implementation for changing notification settings
        }

        public bool notifyDBOrderQueryStatus()
        {
            // Implementation for notifying DB order query status
            return true;
        }

        public List<Product> GetAllProducts()
        {
            return _product.getAllProducts();
        }

        public Product GetOneProduct(int productId)
        {
            return _product.GetProductDetails(productId);
        }

          public List<string> GetShippingAgents(Service shippingType)
        {
            return _shippingAgents.getShippingAgentList(shippingType);
        }

        














        
    }
}