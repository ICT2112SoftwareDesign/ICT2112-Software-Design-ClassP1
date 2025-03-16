using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Domain
{
    public class CarbonFootprintRepositoryControl
    {
        private readonly ICarbonRepositoryQuery _repository;

        public CarbonFootprintRepositoryControl(ICarbonRepositoryQuery repository)
        {
            _repository = repository;
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
