using CleanBrilliantCompany.Interface;
using CleanBrilliantCompany.DatabaseEntities;
using System.Linq;

namespace CleanBrilliantCompany.Service
{
    public class CostDataRetrievalService : CostIItem, CostIManufacturer, CostIBatch
    {
        private readonly ApplicationDbContext _db;

        public CostDataRetrievalService(ApplicationDbContext db)
        {
            _db = db;
        }

        // IItem: Returns raw ItemTable entities
        public List<ItemTable> getItems()
        {
            return _db.Items.ToList();
        }

        // IManufacturer: Returns raw ManufacturerTable entities
        public List<ManufacturerTable> GetAllManufacturers()
        {
            return _db.Manufacturers.ToList();
        }

        // IBatch: Returns raw ProductBatchTable entities
        public List<ProductBatchTable> GetAllProductBatch()
        {
            return _db.ProductBatch.ToList();
        }
    }
}