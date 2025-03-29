using System;
using System.IO;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models
{
    public class WishlistSystemLogger : IWishlistQueryObserver
    {
        private readonly string _logFilePath = "wishlist_system_log.txt";
        
        private void LogToFile(string message)
        {
            try
            {
                string logEntry = $"[{DateTime.Now}] {message}";
                File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                // Log to console if file logging fails
                Console.WriteLine($"Failed to log to file: {ex.Message}");
            }
        }

        public void LogGetWishlist(int customerId)
        {
            LogToFile($"Retrieving wishlist for Customer ID: {customerId}");
        }

        public void LogWishlistExists(int customerId, bool exists)
        {
            LogToFile($"Checking wishlist exists for Customer ID: {customerId}, Result: {exists}");
        }

        public void LogSaveWishlist(int customerId, string productIdsString, bool success)
        {
            LogToFile($"Saving wishlist for Customer ID: {customerId}, Products: {productIdsString}, Success: {success}");
        }
    }
}