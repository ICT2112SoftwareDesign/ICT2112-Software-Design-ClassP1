using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface ICustomerDatabase
    {
        // For login/register
        bool createCustomer(string username, string password, string email);
        bool verifyCustomerCredentials(string email, string password);
        bool customerExists(string email);
        bool customerUsernameExists(string username);

        // For update
        bool customerEmailExists(int customerId, string email);
        bool customerUsernameExists(int customerId, string username);
        bool updateCustomerDetails(int customerId, string username, string email, string address);
        bool updatePassword(int customerId, string password);
        bool updateEmailPreference(int customerId, string? emailPreference);


        // For session
        int getIdByEmail(string email);
        CustomerRDM getCustomer(int customerId);
        
    }
}