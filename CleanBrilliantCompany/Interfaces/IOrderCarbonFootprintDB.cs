using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IOrderCarbonFootprintDB
    {
        bool insertOrderCF(int orderId, string transportMode, double orderWeight, double distance, double carbonEmission, string ecoStatus, DateTime dateCreated);
        double retrieveOrderCarbonFootprint(int orderCFId);
        List<OrderCarbonFootprintRDM> retrieveAllOrderCarbonFootprint();
        float retrieveTotalCarbonFootprint();
        bool getQueryStatus();
    }
}
