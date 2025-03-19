namespace CleanBrilliantCompany.Models
{
    public class CarbonFootprintRecordRDM{
        private int carbonFootprintId;
        private int entityId;
        private string entityType;
        private double carbonEmission;
        private string ecoStatus;
        private DateOnly dateCreated;

        // Constructor
        public CarbonFootprintRecordRDM(int carbonFootprintId, int entityId, string entityType, double carbonEmission, string ecoStatus, DateOnly dateCreated){
            this.carbonFootprintId = carbonFootprintId;
            this.entityId = entityId;
            this.entityType = entityType;
            this.carbonEmission = carbonEmission;
            this.ecoStatus = ecoStatus;
            this.dateCreated = dateCreated;
        }

        // Getters and setters
        private int getCarbonFootprintId()
        {
            return carbonFootprintId;
        }

        private void setCarbonFootprintId(int carbonFootprintId)
        {
            this.carbonFootprintId = carbonFootprintId;
        }

        private int getEntityId()
        {
            return entityId;
        }

        private void setEntityId(int entityId)
        {
            this.entityId = entityId;
        }

        private string getEntityType()
        {
            return entityType;
        }

        private void setEntityType(string entityType)
        {
            this.entityType = entityType;
        }

        private double getCarbonEmission()
        {
            return carbonEmission;
        }

        private void setCarbonEmission(double carbonEmission)
        {
            this.carbonEmission = carbonEmission;
        }

        private string getEcoStatus()
        {
            return ecoStatus;
        }

        private void setEcoStatus(string ecoStatus)
        {
            this.ecoStatus = ecoStatus;
        }

        private DateOnly getDateCreated()
        {
            return dateCreated;
        }

        private void setDateCreated(DateOnly dateCreated)
        {
            this.dateCreated = dateCreated;
        }

        // Public methods
        public string getCarbonFootprintSummary()
        {
            return $"Entity ID: {entityId}, Type: {entityType}, Emission: {carbonEmission}kg, Status: {ecoStatus}, Date: {dateCreated}";
        }

        public void updateCarbonEmission(double carbonEmission)
        {
            this.carbonEmission = carbonEmission;
        }

        public double getCarbonEmissionForCalculation()
        {
            return carbonEmission;
        }

        public bool isEcoFriendly()
        {
            return ecoStatus.Equals("Eco", StringComparison.OrdinalIgnoreCase);
        }

        public bool isDateWithinRange(DateOnly startDate, DateOnly endDate)
        {
            return dateCreated >= startDate && dateCreated <= endDate;
        }

        public int getEntityIdForInsert()
        {
            return entityId;
        }

        public string getEntityTypeForInsert()
        {
            return entityType;
        }

        public string getEcoStatusForInsert()
        {
            return ecoStatus;
        }

        public DateOnly getDateCreatedForInsert()
        {
            return dateCreated;
        }

        public int getCarbonFootprintIdForUpdate()
        {
            return this.carbonFootprintId;
        }

    }
}