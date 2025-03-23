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