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

        public string getEmailPreferenceRaw()
        {
            return emailPreference;
        }

        private void setEmailPreferenceRaw(string value)
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
                default:
                    throw new Exception("Unknown property");
            }
        } 

        // Public method to check if a type should be excluded from emails
        public bool ShouldSuppressEmail(string type) // e.g., "paid" or "cancelled"
        {
            if (string.IsNullOrEmpty(emailPreference)) return false;
            return emailPreference.Split(',').Contains(type);
        }

        // For form binding
        public bool GetPaidSuppressed() => ShouldSuppressEmail("paid");
        public bool GetCancelledSuppressed() => ShouldSuppressEmail("cancelled");

        public void SetPreferencesFromCheckbox(bool suppressPaid, bool suppressCancelled)
        {
            List<string> suppressed = new List<string>();
            if (suppressPaid) suppressed.Add("paid");
            if (suppressCancelled) suppressed.Add("cancelled");
            emailPreference = suppressed.Count > 0 ? string.Join(",", suppressed) : null;
        }
    }
    
}