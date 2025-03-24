using System;
using System.Collections.Generic;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IOrder
    {
        Order GetOrderDetails(int orderId);
        // List<Order> GetOrderHistory(int customerId);
        // bool CancelOrder(int orderId);
    }

    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderAddress { get; set; }
        public Dictionary<int, int> OrderProducts { get; set; } = new Dictionary<int, int>();

        public Dictionary<string, (string ShippingType, string ShippingMethod)> OrderShipping { get; set; } = 
            new Dictionary<string, (string, string)>();

        public List<Item> OrderItems { get; set; } = new List<Item>();
        public string Status { get; set; }
        public float orderTotal { get; set; }
    }

    public class Item
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
    public class MockOrderService : IOrder
    {
        public Order GetOrderDetails(int orderId)
        {
            return new Order
            {
                OrderId = orderId,
                CustomerId = 1,
                OrderDate = DateTime.Now.AddDays(-5),
                OrderAddress = "123 Main Street, City, Country",
                OrderProducts = new Dictionary<int, int>
                {
                    { 14, 2 },  
                    { 18, 1 }   
                },
                OrderShipping = new Dictionary<string, (string, string)>
                {
                    { "FedEx", ("Express", "Air") }
                },
                OrderItems = new List<Item>
                {
                    new Item { ProductId = 14, Quantity = 2},
                    new Item { ProductId = 18, Quantity = 1}
                },
                Status = "Completed",
                orderTotal = 3.00f
            };
        }
    }
}
