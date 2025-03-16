using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Domain
{
    public class CarbonFootprintRepositoryControl : ICarbonRepositoryStatusQuery
    {
        private readonly ICarbonRepositoryQuery _repository;

        public CarbonFootprintRepositoryControl(ICarbonRepositoryQuery repository)
        {
            _repository = repository;
        }

        public bool getDatabaseQueryStatus()
        {
            return _repository.getQueryStatus();
        }

        public float GetProductCarbonFootprint(int entityId, string entityType)
        {
            return _repository.retrieveProductCarbonFootprint(entityId, entityType);
        }

        public float GetOrderCarbonFootprint(int entityId, string entityType)
        {
            return _repository.retrieveOrderCarbonFootprint(entityId, entityType);
        }
    }
}
