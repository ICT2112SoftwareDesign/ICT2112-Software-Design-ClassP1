using System;
using System.Collections.Generic;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models
{
	// A mock implementation of IOrder to simulate retrieval of order details.
	public class MockOrderService : IOrder
	{
		private readonly Dictionary<int, OrderRDM> _orders;

		public MockOrderService()
		{
			_orders = new Dictionary<int, OrderRDM>();

			// Dummy Order 1: Shipping via Air
			OrderRDM order1 = new OrderRDM(
				orderID: 101,
				customerID: 11,
				orderAddress: "Buckingham Palace",
				orderProducts: new Dictionary<int, int> { { 1, 2 }, { 2, 4 } },
				orderShipping: "Air",
				orderItems: new List<int> { 1001, 1002 },
				orderDate: DateTime.Now.AddDays(-1),
				status: "Shipped",
				orderTotal: 100.0m,
				orderWeight: 12.0
			);
			_orders.Add(order1.GetOrderID(), order1);

			// Dummy Order 2: Shipping via Truck
			OrderRDM order2 = new OrderRDM(
				orderID: 102,
				customerID: 22,
				orderAddress: "Jurong Point",
				orderProducts: new Dictionary<int, int> { { 3, 1 } },
				orderShipping: "Truck",
				orderItems: new List<int> { 2001 },
				orderDate: DateTime.Now.AddDays(-2),
				status: "Processing",
				orderTotal: 50.0m,
				orderWeight: 8.0
			);
			_orders.Add(order2.GetOrderID(), order2);

			// Dummy Order 3: Shipping via Sea
			OrderRDM order3 = new OrderRDM(
				orderID: 103,
				customerID: 33,
				orderAddress: "Eiffel Tower",
				orderProducts: new Dictionary<int, int> { { 4, 3 } },
				orderShipping: "Sea",
				orderItems: new List<int> { 3001 },
				orderDate: DateTime.Now.AddDays(-3),
				status: "Delivered",
				orderTotal: 150.0m,
				orderWeight: 20.0
			);
			_orders.Add(order3.GetOrderID(), order3);
		}

		public OrderRDM getOrderDetails(int orderId)
		{
			_orders.TryGetValue(orderId, out var order);
			return order;
		}

		public string GetOrderAddress {get;set;}

		public List<OrderRDM> getAllOrders()
		{
			return _orders.Values.ToList(); // Convert Dictionary values to a List
		}

		public List<int> getOrderItemIds(int orderId)
		{
			if (_orders.TryGetValue(orderId, out var order))
			{
				return order.GetOrderItems();
			}
			return new List<int>();
		}
	}
}
