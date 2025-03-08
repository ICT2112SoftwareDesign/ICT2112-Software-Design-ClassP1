namespace CleanBrilliantCompany.Interfaces
{
    public interface iCustomerDatabase
    {
        bool createCustomer(string username, string password, string email);
        bool updateCustomer(string username, string password, string customerAddress, string email);
        bool VerifyCustomerCredentials(string email, string password);
        bool CustomerExists(string email);
    }
}