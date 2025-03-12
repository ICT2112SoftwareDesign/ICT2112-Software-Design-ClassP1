using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface ICarbonFootprint
    {
        public List<CarbonFootprintRecordRDM> GetAllProductCarbonFootprint();
        public List<CarbonFootprintRecordRDM> GetAllOrderCarbonFootprint();
        public float CalculateCarbonFootprint(int productId, float volume, float toxicity);
        public float CalculateCarbonFootprint(ShipmentSDM shipment);
        public float CalculateCarbonFootprint(int itemId);
    }
}
