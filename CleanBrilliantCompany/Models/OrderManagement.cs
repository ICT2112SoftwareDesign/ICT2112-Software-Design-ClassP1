using System;
using System.Collections.Generic;
using System.Linq;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models
{
    public class OrderManagement : IOrder , IOrderRange
    {
        private readonly IOrderDatabase _orderDatabase;
        private readonly ICartManagement _cartManagement;
        private readonly IShippingAgent _shippingAgent;
        private readonly ISubmitRefund _submitRefund;
        private readonly IOrderFulfilment _orderFulfilment;


        // public OrderManagement(IOrderDatabase orderDatabase, ICartManagement cartManagement, ISubmitRefund submitRefund, IShippingAgent shippingAgent, IOrderFulfilment orderFulfilment)
        public OrderManagement(IOrderDatabase orderDatabase, ICartManagement cartManagement, ISubmitRefund submitRefund, IShippingAgent shippingAgent, IOrderFulfilment orderFulfilment)
        {
            _orderDatabase = orderDatabase;
            _cartManagement = cartManagement;
            _submitRefund = submitRefund;
            _shippingAgent = shippingAgent;
            _orderFulfilment = orderFulfilment;
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

                // Fetch all available shipping agents
                var availableAgents = _shippingAgent.GetAllShippingAgentsAsync().Result;

               // If no valid shippingAgent, shippingType, or serviceType is provided, select a default
                if (string.IsNullOrWhiteSpace(shippingAgent) || string.IsNullOrWhiteSpace(shippingType) || string.IsNullOrWhiteSpace(serviceType))
                {
                    var defaultAgent = availableAgents.FirstOrDefault();
                    if (defaultAgent == null)
                    {
                        throw new Exception("No available shipping agents found.");
                    }

                    // Use the default agent's details
                    shippingAgent = defaultAgent.ShippingAgentCompany;
                    shippingType = defaultAgent.ShippingMethod;
                    serviceType = defaultAgent.ServiceType;

                    Console.WriteLine($"Default shipping agent selected: {shippingAgent}, {shippingType}, {serviceType}");
                }

                // Validate the provided or default shipping agent
                var selectedAgent = availableAgents.FirstOrDefault(agent =>
                    agent.ShippingAgentCompany.Equals(shippingAgent, StringComparison.OrdinalIgnoreCase) &&
                    agent.ShippingMethod.Equals(shippingType, StringComparison.OrdinalIgnoreCase) &&
                    agent.ServiceType.Equals(serviceType, StringComparison.OrdinalIgnoreCase));

                if (selectedAgent == null)
                {
                    throw new Exception("Invalid shipping agent, method, or service type selected.");
                }


                 // Serialize the shipping details into JSON
                var shippingDetails = new
                {
                    ShippingAgent = selectedAgent.ShippingAgentCompany,
                    ShippingMethod = selectedAgent.ShippingMethod,
                    ServiceType = selectedAgent.ServiceType,
                    ShippingFee = shippingFee.ToString("F2")
                };
                string orderShippingJson = System.Text.Json.JsonSerializer.Serialize(shippingDetails);

                // Call adjustInventory to update inventory and get item IDs
                var items = _orderFulfilment.adjustInventory(0, cart); // Pass 0 for orderId initially
                var itemIds = items.Select(item => item.itemId).ToList();


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

        public List<string> getAvailableShippingAgents(string shippingType, string serviceType)
        {

            // Trim and normalize the inputs
            if (string.IsNullOrWhiteSpace(shippingType) || string.IsNullOrWhiteSpace(serviceType))
            {
                return new List<string>();
            }
            shippingType = shippingType.Trim();
            serviceType = serviceType.Trim();

            // Fetch all agents
            var allAgents = _shippingAgent.GetAllShippingAgentsAsync().Result;

            // Filter agents by shipping type and service type
            var filteredAgents = allAgents
                .Where(agent => agent.ShippingMethod.Equals(shippingType, StringComparison.OrdinalIgnoreCase) &&
                                agent.ServiceType.Equals(serviceType, StringComparison.OrdinalIgnoreCase))
                .Select(agent => agent.ShippingAgentCompany)
                .Distinct()
                .ToList();

            return filteredAgents;
        }

        public List<string> getServiceTypes()
        {
            return _shippingAgent.GetAllShippingAgentsAsync().Result
                .Select(agent => agent.ServiceType)
                .Distinct()
                .ToList();
        }

        public List<string> getShippingMethods()
        {
            return _shippingAgent.GetAllShippingAgentsAsync().Result
                .Select(agent => agent.ShippingMethod)
                .Distinct()
                .ToList();
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
            if (order == null || order.RetrieveStatus() == "Cancelled")
            {
                return false;
            }

            order.UpdateStatus("Cancelled");
            return _orderDatabase.updateOrder(order);
        }

        //public bool updateOrderStatus(int orderId, string status)
        //{
        //    var order = _orderDatabase.getOrderById(orderId);
        //    if (order == null)
        //    {
        //        return false;
        //    }

        //    order.UpdateStatus(status);
        //    return _orderDatabase.updateOrder(order);
        //}

        public bool updateOrderStatus(int orderId, string status)
        {
            Console.WriteLine($"Updating order {orderId} to status: {status}");

            var order = _orderDatabase.getOrderById(orderId);
            if (order == null)
            {
                Console.WriteLine("Order not found.");
                return false;
            }

            order.UpdateStatus(status);
            bool result = _orderDatabase.updateOrder(order);

            Console.WriteLine($"Order update success: {result}");
            return result;
        }

        public bool cancelOrder(int orderId, int customerId)
        {
            var order = _orderDatabase.getOrderById(orderId);
            if (order == null || order.RetrieveCustomerID() != customerId || order.RetrieveStatus() != "Pending")
            {
                return false; // Cannot cancel the order
            }
            
            // call the processCancelledOrder from MOD 2
            _orderFulfilment.processCancelledOrder(orderId);

            order.UpdateStatus("Cancelled");
            return _orderDatabase.updateOrder(order);
        }

        public bool requestRefund(int orderId, int customerId, string refundReason)
        {
            var order = _orderDatabase.getOrderById(orderId);
            if (order == null || order.RetrieveCustomerID() != customerId || order.RetrieveStatus() != "Completed")
            {
                return false; // Cannot request a refund
            }

            Console.WriteLine($"{orderId}, {refundReason} {order.RetrieveOrderTotal()}, {order.RetrieveOrderProducts()}, {customerId}");
            order.UpdateStatus("RefundRequested");

            // I can't do this without a concrete implementation of submitRefund yet
            var orderTotal = (float)order.RetrieveOrderTotal();
            var orderProds = order.RetrieveOrderProducts();
            _submitRefund.SubmitRefund(orderId, refundReason,orderTotal, orderProds);
            return _orderDatabase.updateOrder(order);
        }

         // Method for IOrderRange Interface 
        
        // For Mod 2 Team 6 = Get orders by date range
        public List<OrderRDM> getOrdersByDateRange(int monthNumber)
        {
            // Fetch all orders
            var allOrders = _orderDatabase.getAllOrders();

            // Filter orders by the specified month
           return allOrders.Where(order => order.RetrieveOrderDate().Month == monthNumber).ToList();
        }

        // Method for IOrder interface

        // For Mod 1 Team 4 = Get all orders
        public List<OrderRDM> getAllOrders()
        {
            return _orderDatabase.getAllOrders();
        }


        // For Mod 3 Team 1 = Get a list of order items by order ID
         public List<int> getOrderItemIds(int orderId)
        {
            var order = _orderDatabase.getOrderById(orderId);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {orderId} not found.");
            }

            return order.RetrieveOrderItems(); // Assuming OrderRDM has a method GetOrderItems()
        }
    }
}