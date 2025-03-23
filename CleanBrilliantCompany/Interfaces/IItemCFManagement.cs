namespace CleanBrilliantCompany.Interfaces
{
    public interface IItemCFManagement
    {
        bool addItemCF(int itemId, int productId, double carbonEmission, string ecoStatus, DateTime dateCreated);
        bool updateAllItemCF();
    }
}
