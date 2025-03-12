namespace CleanBrilliantCompany.Models
{
    public class ShipmentSDM{
        private int shipmentId;
        private int orderId;
        private double totalWeight;
        private List<RouteSegmentSDM> routeSegments;

        public ShipmentSDM(){

        }

        public List<RouteSegmentSDM> getRouteSegments(){
            return routeSegments;
        }
    }
}