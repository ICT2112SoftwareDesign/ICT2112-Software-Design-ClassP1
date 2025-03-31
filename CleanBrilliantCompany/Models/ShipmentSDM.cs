using System.Collections.Generic;

namespace CleanBrilliantCompany.Models
{
    public class ShipmentSDM
    {
        // Public properties so that control classes can access them.
        public int ShipmentId { get; set; }
        public int OrderId { get; set; }
        public double TotalWeight { get; set; }
        public List<RouteSegment> RouteSegments { get; set; } = new List<RouteSegment>();

        public override string ToString()
        {
            return $"Shipment #{ShipmentId} (Order {OrderId}), Weight: {TotalWeight} kg, Segments: {RouteSegments.Count}";
        }
    }
}
