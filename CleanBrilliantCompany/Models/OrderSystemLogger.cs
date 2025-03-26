using System;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Observers
{
    public class OrderSystemLogger : IOrderQueryObserver
    {
        public void onOrderCreated(int orderId, int customerId, decimal orderTotal)
        {
            // Log the creation of a new order
            Console.WriteLine($"[Order Created] Order ID: {orderId}, Customer ID: {customerId}, Total: {orderTotal:C}");
        }

        public void onOrderUpdated(int orderId, int customerId, string status)
        {
            // Log the update of an order
            Console.WriteLine($"[Order Updated] Order ID: {orderId}, Customer ID: {customerId}, New Status: {status}");
        }

        public void onOrderCancelled(int orderId, int customerId)
        {
            // Log the cancellation of an order
            Console.WriteLine($"[Order Cancelled] Order ID: {orderId}, Customer ID: {customerId}");
        }
    }
}