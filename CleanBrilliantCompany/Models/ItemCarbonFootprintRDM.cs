namespace CleanBrilliantCompany.Models
{
    public class ItemCarbonFootprintRDM
    {
        private int itemCFId;
        private int itemId;
        private int productId;
        private double carbonEmission;
        private string ecoStatus;
        private DateTime dateCreated;

        // Constructor
        public ItemCarbonFootprintRDM(int itemCFId, int itemId, int productId, double carbonEmission, string ecoStatus, DateTime dateCreated)
        {
            this.itemCFId = itemCFId;
            this.itemId = itemId;
            this.productId = productId;
            this.carbonEmission = carbonEmission;
            this.ecoStatus = ecoStatus;
            this.dateCreated = dateCreated;
        }

        // Getters and Setters
        public int getItemCFId()
        {
            return itemCFId;
        }

        private void setItemCFId(int itemCFId)
        {
            this.itemCFId = itemCFId;
        }

        public int getItemId()
        {
            return itemId;
        }

        private void setItemId(int itemId)
        {
            this.itemId = itemId;
        }

        public int getProductId()
        {
            return productId;
        }

        private void setProductId(int productId)
        {
            this.productId = productId;
        }

        private double getCarbonEmission()
        {
            return carbonEmission;
        }

        private void setCarbonEmission(double carbonEmission)
        {
            this.carbonEmission = carbonEmission;
        }

        public string getEcoStatus()
        {
            return ecoStatus;
        }

        private void setEcoStatus(string ecoStatus)
        {
            this.ecoStatus = ecoStatus;
        }

        private DateTime getDateCreated()
        {
            return dateCreated;
        }

        private void setDateCreated(DateTime dateCreated)
        {
            this.dateCreated = dateCreated;
        }

        public double calculateSelfEmission()
        {
            return carbonEmission;
        }

        public DateTime retrieveDateCreated()
        {
            return dateCreated;
        }
    }
}
