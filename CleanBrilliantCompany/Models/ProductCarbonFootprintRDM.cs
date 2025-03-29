namespace CleanBrilliantCompany.Models
{
    public class ProductCarbonFootprintRDM
    {
        private int productCFId;
        private int productId;
        private string productName;
        private string productCategory;
        private double carbonEmission;
        private string ecoStatus;
        private DateTime dateCreated;

        // Constructor
        public ProductCarbonFootprintRDM(int productCFId, int productId, string productName, string productCategory, double carbonEmission, string ecoStatus, DateTime dateCreated)
        {
            this.productCFId = productCFId;
            this.productId = productId;
            this.productName = productName;
            this.productCategory = productCategory;
            this.carbonEmission = carbonEmission;
            this.ecoStatus = ecoStatus;
            this.dateCreated = dateCreated;
        }

        // Getters and Setters
        private int getProductCFId()
        {
            return productCFId;
        }

        private void setProductCFId(int productCFId)
        {
            this.productCFId = productCFId;
        }

        private int getProductId()
        {
            return productId;
        }

        private void setProductId(int productId)
        {
            this.productId = productId;
        }

        private string getProductName()
        {
            return productName;
        }

        private void setProductName(string productName)
        {
            this.productName = productName;
        }

        private string getProductCategory()
        {
            return productCategory;
        }

        private void setProductCategory(string productCategory)
        {
            this.productCategory = productCategory;
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

        public string retrieveProductName()
        {
            return productName;
        }

        public string retrieveProductCategory()
        {
            return productCategory;
        }

        public int retrieveProductId()
        {
            return productId;
        }

        public string retrieveEcoStatus()
        {
            return ecoStatus;
        }
    }
}
