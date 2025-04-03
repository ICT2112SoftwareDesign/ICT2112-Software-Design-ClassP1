using System;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Mappers;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Models
{
    public class CustomerManagement : ICustomerSession
    {
        private readonly ICustomerDatabase _customerDatabase;

        public CustomerManagement(ICustomerDatabase customerDatabase)
        {
            _customerDatabase = customerDatabase;
        }
 
        public bool createAccount(string username, string password, string email)
        {
            // Check if customer already exists
            if (_customerDatabase.customerExists(email))
            {
                return false;
            }
            else if(_customerDatabase.customerUsernameExists(username)){
                return false;
            }
            return _customerDatabase.createCustomer(username, password, email);
        }

        public int getIdByEmail(string email)
        {
            return _customerDatabase.getIdByEmail(email);
        }

        public CustomerRDM getCustomer(int customerId)
        {
            return _customerDatabase.getCustomer(customerId);
        }

        public bool customerEmailExists(int customerId, string email){
            return _customerDatabase.customerEmailExists(customerId, email);
        }

        public bool customerUsernameExists(int customerId, string username){
            return _customerDatabase.customerUsernameExists(customerId, username);
        }

        public bool updateCustomerDetails(int customerId, string username, string email, string address)
        {
            return _customerDatabase.updateCustomerDetails(customerId, username, email, address);
        }

         public bool updatePassword(int customerId, string password)
        {
            return _customerDatabase.updatePassword(customerId, password);
        }  

        public bool authenticateCustomer(string email, string password)
        {
            // Call the method in CustomerMapper to verify credentials
            return _customerDatabase.verifyCustomerCredentials(email, password);
        }
        
        public bool updateEmailPreference(int customerId, string? emailPreference)
        {
            return _customerDatabase.updateEmailPreference(customerId, emailPreference);
        }

    }
}