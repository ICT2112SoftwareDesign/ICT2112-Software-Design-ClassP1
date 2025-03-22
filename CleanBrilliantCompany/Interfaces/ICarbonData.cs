using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface ICarbonData
    {
        // Calculate Carbon Footprint for Product
        float CalculateProductCF(float volume, float toxicPercent, int productId);

        // Calculate Carbon Footprint for Item
        bool CalculateItemCF(int itemId, int productId);

        // Calculate Carbon Footprint for Shipment
        float CalculateShipmentCF(ShipmentSDM shipment);
    }
}