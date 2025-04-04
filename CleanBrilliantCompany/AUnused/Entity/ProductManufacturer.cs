namespace CleanBrilliantCompany.Models.Entity
{
    public class ProductManufacturer
    {
        // CLASS DIAGRAM
        // - manufacturerId: Int
        // - companyName: String
        // - address: String
        // - email: String

        // DB
        //     [manufacturerId]
        //   ,[companyName]
        //   ,[manufacturerAddress]
        //   ,[email]

        private int ManufacturerId;
        private string CompanyName;
        private string ManufacturerAddress;
        private string Email;

        public ProductManufacturer(int manufacturerId, string companyName, string manufacturerAddress, string email)
        {
            ManufacturerId = manufacturerId;
            CompanyName = companyName;
            ManufacturerAddress = manufacturerAddress;
            Email = email;
        }

        public Dictionary<string, object> retrieveProductManufacturerInfo()
        {
            return new Dictionary<string, object>
            {
                { "ManufacturerId", ManufacturerId },
                { "CompanyName", CompanyName },
                { "ManufacturerAddress", ManufacturerAddress },
                { "Email", Email },
            };
        }

         // Private Getters
        private int getManufacturerId() => ManufacturerId;
        private string getCompanyName() => CompanyName;
        private string getManufacturerAddress() => ManufacturerAddress;
        private string getEmail() => Email;

        // Private Setters 
        private void setManufacturerId(int manufacturerId) => ManufacturerId = manufacturerId;
        private void setCompanyName(string companyName) => CompanyName = companyName;
        private void setManufacturerAddress(string manufacturerAddress) => ManufacturerAddress = manufacturerAddress;
        private void setEmail(string email) => Email = email;

        public ProductManufacturer() { }
    }
}