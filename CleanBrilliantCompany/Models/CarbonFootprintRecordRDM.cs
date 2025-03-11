namespace CleanBrilliantCompany.Models
{
    public class CarbonFootprintRecordRDM{
        private int carbonFootprintId;
        private int entityId;
        private string entityType;
        private double carbonEmission;
        private string ecoStatus;
        private DateOnly dateCreated;

        public CarbonFootprintRecordRDM(int carbonFootprintId, int entityId, string entityType, double carbonEmission, string ecoStatus, DateOnly dateCreated){
            this.carbonFootprintId = carbonFootprintId;
            this.entityId = entityId;
            this.entityType = entityType;
            this.carbonEmission = carbonEmission;
            this.ecoStatus = ecoStatus;
            this.dateCreated = dateCreated;
        }

        //getters n setters
    }
}