using System;
using System.Collections.Generic;
using System.Linq;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models
{
    public class OrderManagementStub : IOrder //, IOrderRange
    {
        // private readonly IOrderDatabase _orderDatabase;
        // private readonly ICartManagement _cartManagement;
        // private readonly IShippingAgents _shippingAgents;

        public OrderManagementStub(/***IOrderDatabase orderDatabase, ICartManagement cartManagement, IShippingAgents shippingAgents***/)
        {
            // _orderDatabase = orderDatabase;
            // _cartManagement = cartManagement;

            // _shippingAgents = shippingAgents;
        }

        // public int createOrder(
        //     int customerId,
        //     string deliveryAddress,
        //     string serviceType,
        //     string shippingType,
        //     string shippingAgent,
        //     Dictionary<int, int> cart)
        // {
        //     try
        //     {
        //         // Fetch product details for the cart
        //         var products = _cartManagement.getCartProductDetails(cart);
        //         if (products == null || products.Count == 0)
        //         {
        //             throw new Exception("Failed to fetch product details for the cart.");
        //         }

        //         // Calculate the cart total
        //         decimal cartTotal = _cartManagement.calculateCartTotal(cart, products);

        //         // Calculate the shipping fee
        //         decimal shippingFee = calculateShippingFee(serviceType);

        //         // Serialize the shipping details into JSON
        //         var shippingDetails = new
        //         {
        //             ShippingAgent = shippingAgent,
        //             ShippingMethod = shippingType,
        //             ServiceType = serviceType,
        //             ShippingFee = shippingFee.ToString("F2")
        //         };
        //         string orderShippingJson = System.Text.Json.JsonSerializer.Serialize(shippingDetails);

        //          // Call adjustInventory to update inventory and get item IDs
        //         //var items = _orderFulfilment.adjustInventory(0, cart); // Pass 0 for orderId initially
        //         //var itemIds = items.Select(item => item.itemId).ToList();


        //         // Create the order object
        //         var order = new OrderRDM(
        //             orderID: 0, // Will be set by the database
        //             customerID: customerId,
        //             orderAddress: deliveryAddress,
        //             orderProducts: cart,
        //             orderShipping: orderShippingJson,
        //             orderItems: cart.Keys.ToList(), // Use product IDs as serial numbers
        //             orderDate: DateTime.Now,
        //             status: "Pending",
        //             orderTotal: cartTotal + shippingFee
        //         );

        //         // Save the order to the database
        //         return _orderDatabase.insertOrder(order);
        //     }
        //     catch (Exception ex)
        //     {
        //         // Log the error and rethrow or handle it
        //         throw new Exception($"Failed to create order: {ex.Message}");
        //     }
        // }

        //   // Fetch available shipping agents for a given shipping type
        // public List<string> getAvailableShippingAgents(string shippingType)
        // {
        //     var selectedServiceEnum = Enum.TryParse<Service>(shippingType, out var serviceEnum) ? serviceEnum : Service.OneDay;
        //     return _shippingAgents.getShippingAgentList(selectedServiceEnum);
        // }

        // // Fetch available service types
        // public List<string> getServiceTypes()
        // {
        //     return _shippingAgents.getServiceTypes();
        // }

        // // Fetch available shipping methods
        // public List<string> getShippingMethods()
        // {
        //     return _shippingAgents.getShippingMethods();
        // }

        // public decimal calculateShippingFee(string serviceType)
        // {
        //     var serviceCosts = new Dictionary<string, decimal>
        //     {
        //         { "1 Day", 10.00m },
        //         { "3 Days", 5.00m },
        //         { "7 Days", 0.00m }
        //     };

        //     if (!serviceCosts.ContainsKey(serviceType))
        //     {
        //         throw new Exception("Invalid service type selected.");
        //     }

        //     return serviceCosts[serviceType];
        // }

        public OrderRDM getOrderDetails(int orderId)
        {
            // put bunch of stubs in here so app doesn't crash
            // to be populated with actual stubs

            Dictionary<int, int> dict = new Dictionary<int, int>();
            List<int> list = new List<int>();

            return new OrderRDM(
                1,
                1,
                dict,
                "order shipping",
                list,
                DateTime.Now,
                "status",
                100
            );
            //return _orderDatabase.getOrderById(orderId);
        }

        public List<OrderRDM> getAllOrders()
        {
            return new List<OrderRDM>();
            //return _orderDatabase.getAllOrders();
        }


        // For Mod 3 Team 1 = Get a list of order items by order ID
         public List<int> getOrderItemIds(int orderId)
        {
            // var order = _orderDatabase.getOrderById(orderId);
            // if (order == null)
            // {
            //     throw new KeyNotFoundException($"Order with ID {orderId} not found.");
            // }

            return new List<int>();
            // return order.GetOrderItems(); // Assuming OrderRDM has a method GetOrderItems()
        }
    }
}