using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Mappers;
using CleanBrilliantCompany.Models.Entity;
using Microsoft.Extensions.Configuration;

namespace CleanBrilliantCompany.Models.Control
{

    public class ItemControl : IItem, IItemUpdate, IReserve
    {

        private readonly IProduct _product;
        private readonly ItemMapper _itemMapper;

        public ItemControl(IProduct product)
        {
            _product = product;
        }

        public async Task<Item> GetItemById(int itemId, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("CleanBrillantCompany:ConnectionString");
            ItemMapper itemMapper = new ItemMapper(connectionString);
            try
            {
                Item result = await itemMapper.findByItemId(itemId);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating item: {ex.Message}");
                return null;
            }
        }

        public List<Item> GetItemsByStatus(Status status, IConfiguration configuration)
        {
            throw new NotImplementedException();
        }

        public Product RetrieveProductDetails(int productId, IConfiguration configuration)
        {
            Product product = _product.GetProductDetails(productId);
            return product;
        }

        public async Task<bool> UpdateItemById(int itemId, int productId, DateOnly expiryDate, DateOnly receiveDate, DateOnly manufactureDate,
                    float salePrice, int batchCode, int warehouseId, Status status, int reservationId, int orderId,
                    int transferId, int returnId, IConfiguration configuration)
        {
            Item item = new Item();
            item.ItemId = itemId;
            item.ProductId = productId;
            item.ExpiryDate = expiryDate;
            item.ReceiveDate = receiveDate;
            item.ManufactureDate = manufactureDate;
            item.SalePrice = salePrice;
            item.BatchCode = batchCode;
            item.WarehouseId = warehouseId;
            item.Status = status;
            item.ReservationId = reservationId;
            item.OrderId = orderId;
            item.TransferId = transferId;
            item.ReturnId = returnId;
            string connectionString = configuration.GetConnectionString("CleanBrillantCompany:ConnectionString");
            ItemMapper itemMapper = new ItemMapper(connectionString);
            try
            {
                string result = await itemMapper.update(item.ItemId, item.ProductId, item.ExpiryDate, item.ReceiveDate,
                                                    item.ManufactureDate, item.SalePrice, item.BatchCode,
                                                    item.WarehouseId, item.Status, item.ReservationId, item.OrderId,
                                                    item.TransferId, item.ReturnId);
                Console.WriteLine(result);
                return true;
            } catch (Exception ex) {
                Console.WriteLine($"Error updating item: {ex.Message}");
                return false;
            }
        }
    }

}
