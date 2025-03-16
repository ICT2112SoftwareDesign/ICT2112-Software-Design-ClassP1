using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Domain
{
    public class CarbonFootprintManagerControl : ICarbonManagerQuery
    {
        private readonly ICarbonRepositoryQuery _repository;

        public CarbonFootprintManagerControl(ICarbonRepositoryQuery repository)
        {
            _repository = repository;
        }

        public bool getDatabaseQueryStatus()
        {
            return _repository.getQueryStatus();
        }

        public void AddCarbonFootprintRecord(int entityId, string entityType, float carbonEmission, string ecoStatus, DateTime dateCreated)
        {
            _repository.createCarbonFootprint(entityId, entityType, carbonEmission, ecoStatus, dateCreated);
        }

        public void RemoveCarbonFootprintRecord(int carbonFootprintId)
        {
            _repository.deleteCarbonFootprint(carbonFootprintId);
        }

        public void UpdateCarbonFootprintRecord(int carbonFootprintId, int entityId, string entityType, float carbonEmission, string ecoStatus)
        {
            _repository.updateCarbonFootprint(carbonFootprintId, entityId, entityType, carbonEmission, ecoStatus);
        }
    }
}
