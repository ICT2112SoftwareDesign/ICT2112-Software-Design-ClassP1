using System.Collections.Generic;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IManufacturer
    {
        ProductManufacturer getManufacturerDetails(int manufacturerId);
        List<ProductManufacturer> getAllManufacturers();
    }
}