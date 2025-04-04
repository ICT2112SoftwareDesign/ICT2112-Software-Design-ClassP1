using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IOrderCF
    {
        double getOrderCarbonFootprint(int orderCFId);
        List<OrderCarbonFootprintRDM> getAllOrderCarbonFootprint();
        float getTotalCarbonFootprint();
    }
}
