namespace CleanBrilliantCompany.Interfaces
{
    public interface IOrderCFManagement
    {
        bool addOrderCF(int orderId, string transportMode, double orderWeight, double distance, double carbonEmission, string ecoStatus, DateTime dateCreated);
    }
}
