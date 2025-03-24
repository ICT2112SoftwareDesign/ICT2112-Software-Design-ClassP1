using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IWarehouse
    {
        Task <Warehouse> getWarehouseDetails(int warehouseId);
        
        Task<List<Item>> getItemByProductAndWarehouse(int productId, int warehouseId);

        Task<int> getProductQuantityByWarehouse(int productId, int warehouseId);
    }
}