namespace CleanBrilliantCompany.Models
{
    public class Shipment(){
        private int shipmentId;
        private int orderId;
        private double totalWeight;
        private List<RouteSegment> routeSegments;

        public Shipment(){
        //constructor
        }
    }
}