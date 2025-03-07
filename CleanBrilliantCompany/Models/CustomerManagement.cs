using System;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Mappers;

namespace CleanBrilliantCompany.Models
{
    public class CustomerManagement
    {
        private readonly iCustomerDatabase _customerDatabase;

        public CustomerManagement(iCustomerDatabase customerDatabase)
        {
            _customerDatabase = customerDatabase;
        }

        public bool createAccount(string username, string password, string email)
        {
            // Implementation logic here
            return false;
        }

        public string queryChatbot()
        {
            // Implementation logic here
            return string.Empty;
        }

        public CustomerRDM getCustomer(int customerId)
        {
            // Implementation logic here
            return null;
        }

        public bool UpdateCustomerAddress(int customerId, string address)
        {
            // Implementation logic here
            return false;
        }

        public bool notifyDBCustomerQueryStatus()
        {
            // Implementation logic here
            return false;
        }

        public bool AuthenticateCustomer(string email, string password)
        {
            // Call the method in CustomerMapper to verify credentials
            return _customerDatabase.VerifyCustomerCredentials(email, password);
        }
    }
}