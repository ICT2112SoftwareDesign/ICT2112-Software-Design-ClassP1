using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface ICarbonRepositoryQuery
    {
        public bool createCarbonFootprint(int entityId, string entityType, float carbonEmission, string ecoStatus, DateTime dateCreated);
        public bool deleteCarbonFootprint(int carbonFootprintId);
        public bool updateCarbonFootprint(int carbonFootprintId, int entityId, string entityType, float carbonEmission, string ecoStatus);
        public float retrieveProductCarbonFootprint(int entityId, string entityType);
        public float retrieveOrderCarbonFootprint(int entityId, string entityType);
        public List<CarbonFootprintRecordRDM> retrieveAllProductCarbonFootprint();
        public List<CarbonFootprintRecordRDM> retrieveAllOrderCarbonFootprint();
        public bool getQueryStatus();
    }
}
