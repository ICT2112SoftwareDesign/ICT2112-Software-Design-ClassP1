using CleanBrilliantCompany.Models;
namespace CleanBrilliantCompany.Interfaces
{
    public interface ICustomerDatabase
    {
        // For login/register
        bool createCustomer(string username, string password, string email);
        bool VerifyCustomerCredentials(string email, string password);
        bool CustomerExists(string email);

        // For update
        bool customerEmailExists(int customerId, string email);
        bool customerUsernameExists(int customerId, string username);
        bool updateCustomerDetails(string username, string email, string address);

        // For session
        int GetIdByEmail(string email);
        CustomerRDM getCustomer(int loggedInId);
        
    }
}