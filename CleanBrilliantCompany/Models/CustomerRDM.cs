using System;

namespace CleanBrilliantCompany.Models
{
    public class CustomerRDM
    {
        private int customerId;
        private string username;
        private string password;
        private string customerAddress;
        private string email;

        private int getCustomerId()
        {
            return customerId;
        }

        private void setCustomerId(int customerId)
        {
            this.customerId = customerId;
        }

        private string getUsername()
        {
            return username;
        }

        private void setUsername(string username)
        {
            this.username = username;
        }

        private string getPassword()
        {
            return password;
        }

        private void setPassword(string pwd)
        {
            this.password = pwd;
        }

        private string getEmail()
        {
            return email;
        }

        private void setEmail(string email)
        {
            this.email = email;
        }

        private string getCustomerAddress()
        {
            return customerAddress;
        }

        private void setCustomerAddress(string address)
        {
            this.customerAddress = address;
        }

        public bool createCustomer(int customerId, string username, string password, string email)
        {
            // Implementation logic here
            return false;
        }

        public bool getCustDetailsForOrder(int customerId, string username, string password, string email, string address)
        {
            // Implementation logic here
            return false;
        }

        public string fetchReviewUsername()
        {
            // Implementation logic here
            return string.Empty;
        }

        public bool updateCustomerAddress(int customerId, string address)
        {
            // Implementation logic here
            return false;
        }
    }
}