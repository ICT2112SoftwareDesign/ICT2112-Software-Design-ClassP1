using CleanBrilliantCompany.Models.Entity;
namespace CleanBrilliantCompany.Interfaces
{
    public interface ICustomerDatabase
    {
        bool createCustomer(string username, string password, string email);
        // bool updateCustomer(string username, string password, string customerAddress, string email);
        bool updateCustomer(int customerId, string field, string value);
        bool VerifyCustomerCredentials(string email, string password);
        bool CustomerExists(string email);
        int GetIdByEmail(string email);

        CustomerRDM getCustomer(int loggedInId);
    }
}