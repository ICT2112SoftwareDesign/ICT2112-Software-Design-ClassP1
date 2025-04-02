using System;
using System.IO;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models
{
    public class CustomerSystemLogger : ICustomerQueryObserver
    {
        private readonly string _logFilePath = "customer_system_log.txt";
        
        public void onAuthenticationAttempt(string email)
        {
            LogToFile($"[{DateTime.Now}] Authentication attempt by user: {email}");
        }
        
        public void onAuthenticationSuccess(string email, int customerId)
        {
            LogToFile($"[{DateTime.Now}] Authentication succeeded for user: {email} (ID: {customerId})");
        }
        
        public void onAuthenticationFailure(string email, string reason)
        {
            LogToFile($"[{DateTime.Now}] Authentication failed for user: {email}. Reason: {reason}");
        }
        
        public void onCustomerRegistration(string username, string email)
        {
            LogToFile($"[{DateTime.Now}] New customer registered - Username: {username}, Email: {email}");
        }
        public void onUpdateDetailsSuccess(int customerId, string username, string email, string address)
        {
            LogToFile($"[{DateTime.Now}] Update successful for fields for customer {customerId}: (Username: {username}, Email: {email}, Address: {address})");
        }
        public void onUpdatedDetailsFailure(int customerId, string username, string email, string address, string reason)
        {
            LogToFile($"[{DateTime.Now}] Update failed for fields for customer {customerId}: (Username: {username}, Email: {email}, Address: {address}). Error: {reason}");
        }
        public void onUpdatePasswordSuccess(int customerId, string password)
        {
            LogToFile($"[{DateTime.Now}] Update successful for password for customer {customerId}");
        }
        public void onUpdatePasswordFailure(int customerId, string password, string reason)
        {
            LogToFile($"[{DateTime.Now}] Update failed for password for customer {customerId}. Error: {reason}");
        }
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