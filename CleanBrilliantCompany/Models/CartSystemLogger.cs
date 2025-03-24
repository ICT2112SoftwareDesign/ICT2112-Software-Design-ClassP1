using System;
using System.IO;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models
{
    
    // Logs cart-related events to a file for tracking and debugging purposes.
    // Implements the ICartObserver interface to observe cart events.
    public class CartSystemLogger : ICartQueryObserver
    {
        // Path to the log file
        private readonly string _logFilePath = "cart_system_log.txt";

    
        // Logs when a product is added to the cart.

        public void onProductAdded(int customerId, int productId, int quantity)
        {
            LogToFile($"[{DateTime.Now}] Product added to cart - Customer ID: {customerId}, Product ID: {productId}, Quantity: {quantity}");
        }

       
        // Logs when a product quantity is updated in the cart.
        public void onProductQuantityUpdated(int customerId, int productId, int quantity)
        {
            LogToFile($"[{DateTime.Now}] Product quantity updated in cart - Customer ID: {customerId}, Product ID: {productId}, New Quantity: {quantity}");
        }

     
        // Logs when a product is removed from the cart.
        public void onProductRemoved(int customerId, int productId)
        {
            LogToFile($"[{DateTime.Now}] Product removed from cart - Customer ID: {customerId}, Product ID: {productId}");
        }

       
        // Logs when the cart is cleared for a customer.
        public void onCartCleared(int customerId)
        {
            LogToFile($"[{DateTime.Now}] Cart cleared - Customer ID: {customerId}");
        }

       
        // Logs when the cart is updated (general event).
        public void cartUpdated()
        {
            LogToFile($"[{DateTime.Now}] Cart updated.");
        }

     
        // Writes a log message to the log file.
        private void LogToFile(string message)
        {
            try
            {
                using (StreamWriter writer = File.AppendText(_logFilePath))
                {
                    writer.WriteLine(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }
    }
}