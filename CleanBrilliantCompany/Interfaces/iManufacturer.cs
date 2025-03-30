using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface iManufacturer
    {
        public ProductManufacturer getManufacturerDetails(int manufacturerId);
        public List<ProductManufacturer> getAllProductManufacturer();
    }
}