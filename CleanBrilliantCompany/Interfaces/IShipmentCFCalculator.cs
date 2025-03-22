using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IShipmentCFCalculator
    {
        float CalculateCarbonFootprint(ShipmentSDM shipment);
    }
}
