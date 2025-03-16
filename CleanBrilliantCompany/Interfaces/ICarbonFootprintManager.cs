using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface ICarbonFootprintManager
    {
        public void addCarbonFootprintRecord(int entityId, string entityType, float carbonEmission, string ecoStatus);
        public void removeCarbonFootprintRecord(int carbonFootprintId);
        public void updateCarbonFootprintRecord(int carbonFootprintId, int entityId, string entityType, float carbonEmission, string ecoStatus);
        public float calculateCarbonFootprint(int productId, float volume, float toxicity);
        public float calculateCarbonFootprint(ShipmentSDM shipment);
        public float calculateCarbonFootprint(int itemId);
    }
}
