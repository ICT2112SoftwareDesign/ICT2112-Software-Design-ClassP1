using System;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Mappers;

namespace CleanBrilliantCompany.Models
{
    public class CustomerManagement
    {
        private readonly ICustomerDatabase _customerDatabase;

        public CustomerManagement(ICustomerDatabase customerDatabase)
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

        public int GetIdByEmail(string email)
        {
            return _customerDatabase.GetIdByEmail(email);
        }

        public CustomerRDM getCustomer(int loggedInId)
        {
            return _customerDatabase.getCustomer(loggedInId);
        }

        public bool customerEmailExists(int customerId, string email){
            return _customerDatabase.customerEmailExists(customerId, email);
        }

        public bool customerUsernameExists(int customerId, string username){
            return _customerDatabase.customerUsernameExists(customerId, username);
        }

        public bool updateCustomerDetails(string username, string email, string address)
        {
            return _customerDatabase.updateCustomerDetails(username, email, address);
        }

         public bool UpdatePasswordDetails(string password)
        {
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