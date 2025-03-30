using System;
using System.IO;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Observers
{
    public class OrderSystemLogger : IOrderQueryObserver
    {
        // Path to the log file
        private readonly string _logFilePath = "Order_system_log.txt";

        public void onOrderCreated(int orderId, int customerId, decimal orderTotal)
        {
            // Log the creation of a new order
            logtoFile($"[Order Created] Order ID: {orderId}, Customer ID: {customerId}, Total: {orderTotal:C}");
        }

        public void onOrderUpdated(int orderId, int customerId, string status)
        {
            // Log the update of an order
            logtoFile($"[Order Updated] Order ID: {orderId}, Customer ID: {customerId}, New Status: {status}");
        }

        public void onOrderCancelled(int orderId, int customerId)
        {
            // Log the cancellation of an order
            logtoFile($"[Order Cancelled] Order ID: {orderId}, Customer ID: {customerId}");
        }

        // Method to log messages to a file
        private void logtoFile(string message)
        {
            try
            {
                // Add a timestamp to the log message
                string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";

                // Append the log message to the file
                File.AppendAllText(_logFilePath, logMessage + Environment.NewLine);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during logging
                Console.WriteLine($"Failed to write to log file: {ex.Message}");
            }
        }
    }
}