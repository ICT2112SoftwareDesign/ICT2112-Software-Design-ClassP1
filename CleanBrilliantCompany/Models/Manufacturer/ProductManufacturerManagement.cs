using System.Collections.Generic;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models
{
    public class ProductManufacturerManagement : IManufacturer
    {
        private List<ProductManufacturer> manufacturers = new List<ProductManufacturer>
        {
            new ProductManufacturer(101, "EcoTech Industries", "123 Punggol Road, Waterway Point", "contact@ecotech.com"),
            new ProductManufacturer(102, "SmartProducts Pte Ltd", "18 Tai Seng", "contact@smartproducts.com"),
            new ProductManufacturer(103, "Procter & Gamble", "1 Procter & Gamble Plaza, Cincinnati, OH, USA", "contact@pg.com"),
            new ProductManufacturer(104, "Unilever", "Unilever House, 100 Victoria Embankment, London, UK", "info@unilever.com")
        };

        public ProductManufacturer getManufacturerDetails(int manufacturerId)
        {
            return manufacturers.Find(p => p.GetManufacturerID() == manufacturerId) 
                   ?? new ProductManufacturer(101, "ManufacturerID", "Address", "email@contact.com");
        }

        public List<ProductManufacturer> getAllManufacturers()
        {
            return manufacturers;
        }
    }
}