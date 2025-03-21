using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CleanBrilliantCompany.Models.CalculatorImplementation
{
    public class CalculateShipmentCFImpl
    {
        // Calculate the carbon footprint for a shipment.
        public float CalculateCarbonFootprint(ShipmentSDM shipment)
        {
            float totalEmission = 0.0f;

            // Iterate over each route segment of the shipment.
            foreach (var segment in shipment.RouteSegments)
            {
                // Determine the emission factor based on the transport mode.
                float factor = segment.Mode switch
                {
                    TransportMode.Air => 1.5f,
                    TransportMode.Sea => 0.1f,
                    TransportMode.Truck => 0.5f,
                    _ => 1.0f,
                };

                // Calculate emission for the segment and add it to the total.
                totalEmission += segment.Distance * (float)shipment.TotalWeight * factor;
            }

            return totalEmission;
        }
    }
}