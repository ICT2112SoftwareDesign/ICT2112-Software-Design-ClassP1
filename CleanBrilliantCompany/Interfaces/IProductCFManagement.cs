using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IProductCFManagement
    {
        bool addProductCF(int productId, string productName, string productCategory, double carbonEmission, string ecoStatus, DateTime dateCreated);
    }
}
