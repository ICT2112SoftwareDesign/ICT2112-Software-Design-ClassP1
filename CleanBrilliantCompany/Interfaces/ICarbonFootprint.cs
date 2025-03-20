using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface ICarbonFootprint
    {
        public List<CarbonFootprintRecordRDM> getAllProductCarbonFootprint();
        public List<CarbonFootprintRecordRDM> getAllOrderCarbonFootprint();
        public List<CarbonFootprintRecordRDM> getCarbonFootprintComparison(List<int> entityId, string entityType);
        public List<CarbonFootprintRecordRDM> getEcoFriendlyReport(DateTime startDate, DateTime endDate, string entityType);
    }
}