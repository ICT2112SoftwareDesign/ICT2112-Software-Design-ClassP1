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
            // Check if customer already exists
            if (_customerDatabase.CustomerExists(email))
            {
                return false;
            }
            return _customerDatabase.createCustomer(username, password, email);
        }

        public string queryChatbot()
        {
            // Implementation logic here
            return string.Empty;
        }

        // public CustomerRDM getCustomer(int customerId)
        // {
        //     return null;
        // }

        public CustomerRDM getCustomer(string email)
        {
            return _customerDatabase.getCustomer(email);
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