using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface ICarbonData
    {
        float calculateProductCF(float volume, float toxicPercent);
        float calculateItemCF(int itemId);
        float calculateShipmentCF(ShipmentSDM shipment);
    }
}