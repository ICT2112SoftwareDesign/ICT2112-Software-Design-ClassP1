using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface ICustomerSession
    {
        CustomerRDM getCustomer(int customerId);
    }
}