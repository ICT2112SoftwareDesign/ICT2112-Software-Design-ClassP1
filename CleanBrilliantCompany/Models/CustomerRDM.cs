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
        private string emailPreference;

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

        private string getEmailPreference()
        {
            return emailPreference;
        }

        private void setEmailPreference(string value)
        {
            this.emailPreference = value;
        }

        public string getEmailPreferenceRaw()
        {
            return emailPreference;
        }

        public void setEmailPreferenceRaw(string value)
        {
            this.emailPreference = value;
        }

        public T getSession<T>(string propertyName)
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
                case "emailPreference":
                    return (T)(object)getEmailPreference();
                default:
                    throw new Exception("Unknown property");
            }
        }

        // Set method
        public void setSession<T>(string propertyName, T value)
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
                case "emailPreference":
                    setEmailPreference(value?.ToString());
                    break;
                default:
                    throw new Exception("Unknown property");
            }
        } 
    }
    
}