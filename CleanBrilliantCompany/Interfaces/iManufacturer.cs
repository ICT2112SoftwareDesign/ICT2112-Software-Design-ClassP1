using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IManufacturer
    {
        public ProductManufacturer getManufacturerDetails(int manufacturerId);
        public List<ProductManufacturer> getAllProductManufacturer();
    }
}