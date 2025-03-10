namespace CleanBrilliantCompany.Interfaces
{
    public interface ICarbonFootprint
    {
        public List<CarbonFootprintRecordRDM> getAllProductCarbonFootprint();
        public List<CarbonFootprintRecordRDM> getAllOrderCarbonFootprint();
        public float calculateCarbonFootprint(int productId, float vol, float tox);
        public float calculateCarbonFootprint(int orderId, List<RouteSegment> route, float totalWeight);
        public float calculateCarbonFootprint(int itemId);
    }
}
