using System;

namespace CleanBrilliantCompany.Models
{
    public class ProductManufacturer
    {
        private int manufacturerId;
        private string companyName;
        private string manufacturerAddress;
        private string email;

        private int GetManufacturerId() => manufacturerId;
        private void SetManufacturerId(int value) => manufacturerId = value;

        private string GetCompanyName() => companyName;
        private void SetCompanyName(string value) => companyName = value;

        private string GetManufacturerAddress() => manufacturerAddress;
        private void SetManufacturerAddress(string value) => manufacturerAddress = value;

        private string GetEmail() => email;
        private void SetEmail(string value) => email = value;

        public ProductManufacturer(int manufacturerId, string companyName, string manufacturerAddress, string email)
        {
            this.manufacturerId = manufacturerId;
            this.companyName = companyName;
            this.manufacturerAddress = manufacturerAddress;
            this.email = email;
        }

        public Dictionary<string, object> GetManufacturerDetails()
        {
            return new Dictionary<string, object>
            {
                { "ManufacturerId", manufacturerId },
                { "CompanyName", companyName },
                { "ManufacturerAddress", manufacturerAddress },
                { "Email", email }
            };
        }

        public int GetManufacturerID()
        {
            return manufacturerId;
        }
    }
}