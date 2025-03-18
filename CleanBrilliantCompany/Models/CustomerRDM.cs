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

        public T GetSession<T>(string propertyName)
        {
            switch (propertyName)
            {
                case "customerId":
                    return (T)(object)getCustomerId();
                case "username":
                    return (T)(object)getUsername();
                case "password":
                    return (T)(object)getPassword();
                case "email":
                    return (T)(object)getEmail();
                case "customerAddress":
                    return (T)(object)getCustomerAddress();
                default:
                    throw new Exception("Unknown property");
            }
        }

        // Set method
        public void SetSession<T>(string propertyName, T value)
        {
            switch (propertyName)
            {
                case "customerId":
                    setCustomerId(Convert.ToInt32(value));
                    break;
                case "username":
                    setUsername(value?.ToString()); 
                    break;
                case "password":
                    setPassword(value?.ToString());
                    break;
                case "email":
                    setEmail(value?.ToString()); 
                    break;
                case "customerAddress":
                    setCustomerAddress(value?.ToString()); 
                    break;
                default:
                    throw new Exception("Unknown property");
            }
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