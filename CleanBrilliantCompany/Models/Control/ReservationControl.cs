using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Mappers;
using CleanBrilliantCompany.Models.Entity;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CleanBrilliantCompany.Models.Control
{
    public class ReservationControl
    {
        private readonly IItem _item;
        private readonly IItemUpdate _itemUpdate;
        private readonly IReserve _reserve;
        private readonly IStaffDetails _staffDetails;
        private readonly ReservationMapper _reservationMapper;

        public ReservationControl(IItem item, IItemUpdate itemUpdate, IReserve reserve, ReservationMapper reservationMapper)
        {
            _item = item;
            _itemUpdate = itemUpdate;
            _reserve = reserve;
            _reservationMapper = reservationMapper;
        }

        public ReservationControl(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");
            _reservationMapper = new ReservationMapper(connectionString);

            Console.WriteLine("Reservation loaded from database.");
        }

        public async Task<List<Reservation>> ShowReservations()
        {
            return await Task.FromResult(_reservationMapper.findAllReservations());
        }

        public async Task<Reservation> GetReservationById(int reservationId)
        {
            Reservation reservation = _reservationMapper.findByReservationId(reservationId);
            List<Item> items = await _reserve.getItemsByStatus(ItemStatus.Reserved);
            //items.RemoveAll(items => items.ReservationId != reservationId);
            items.RemoveAll(item =>
            {
                var itemInfo = item.retrieveItemInfo();
                if (itemInfo.TryGetValue("ReservationId", out var reservationIdObj) && reservationIdObj is int currentReservationId)
                {
                    return currentReservationId != reservationId;
                }
                // Handle the case where "ReservationId" is not in the dictionary or is not an int
                return false; // Or throw an exception, depending on your error handling strategy
            });
            reservation.InsertItems(reservationId, items);
            return await Task.FromResult(reservation);

        }

        public async Task<String> ReserveStock(int reservedQuantity, int warehouseId, int productId, String reservationPurpose, int staffId)
        {
            ItemStatus getStatus = ItemStatus.Available;
            ItemStatus setStatus = ItemStatus.Reserved;
            int itemCount = reservedQuantity;
            int reservationId = await _reservationMapper.GetNextId();
            DateOnly reservationDate = DateOnly.FromDateTime(DateTime.Now);
            List<Item> availableItems = await _reserve.getItemsByStatus(getStatus);
            List<Item> sortedItems = availableItems.OrderBy(x => { x.retrieveItemInfo().TryGetValue("ProductId", out var productIdObj); return productIdObj as int?;}).ThenByDescending(x => { x.retrieveItemInfo().TryGetValue("ExpiryDate", out var expiryDateObj); return expiryDateObj as DateTime?; }).ToList();
            List<Item> reservedItems = new List<Item>();
            foreach (Item item in sortedItems)
            {
                if (item.retrieveItemInfo().TryGetValue("ProductId", out var productIdObj) && productIdObj is int iproductId && iproductId == productId && itemCount >= 1)
                {
                    reservedItems.Add(item);
                    itemCount--;
                }
                else if (itemCount == 0)
                {
                    break;
                }
            }
            foreach (Item item in reservedItems)
            {
                try
                {
                    if (item.retrieveItemInfo().TryGetValue("ItemId", out var itemIdObj) && itemIdObj is int itemId)
                    {
                        bool updatestatus = await _itemUpdate.updateItemStatus(itemId, reservationId, null, null, null, setStatus);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating item: {ex.Message}");
                }
            }
            string result = await _reservationMapper.insert(reservationId, productId, warehouseId, reservationDate, reservationPurpose, reservedQuantity, staffId);

            return result;
        }

        public async Task<string> UpdateReservationQuantity(int reservationId, int quantity, int staffId)
        {
            Reservation reservation = await GetReservationById(reservationId);
            Dictionary<string, object> reservationDict = reservation.GetReservationDetails();
            int productId = 0, warehouseId = 0, reservedQuantity = 0;
            ItemStatus getStatus = ItemStatus.Available;
            ItemStatus setStatus = ItemStatus.Reserved;
            string reservationPurpose = "";
            List<Item> reservedItems = [];
            DateOnly reservationDate = DateOnly.FromDateTime(DateTime.Now);
            if(reservationDict.TryGetValue("ProductId", out var productIdObj) && productIdObj is int tempProductId) {productId = tempProductId;}
            if(reservationDict.TryGetValue("WarehouseId", out var warehouseIdObj) && warehouseIdObj is int tempWarehouseId) {warehouseId = tempWarehouseId;}
            if(reservationDict.TryGetValue("ReservationPurpose", out var reservationPurposeObj) && reservationPurposeObj is string tempReservationPurpose) {reservationPurpose = tempReservationPurpose;}
            if(reservationDict.TryGetValue("ReservedQuantity", out var reservedQuantityObj) && reservedQuantityObj is int tempReservedQuantity) {reservedQuantity = tempReservedQuantity;}
            if(reservationDict.TryGetValue("ReservedItems", out var reservedItemsObj) && reservedItemsObj is List<Item> tempReservedItems) {reservedItems = tempReservedItems;}
            if (quantity != reservedQuantity)
            {
                if (quantity == 0)
                {
                    bool i = await ReturnReservedStockToinventory(reservation, staffId);
                }
                else if (quantity < reservedQuantity)
                {
                    for(int i = reservedQuantity - quantity; i >0;)
                    {
                        if(reservedItems[reservedItems.Count - 1].retrieveItemInfo().TryGetValue("ItemId", out var itemIdObj) && itemIdObj is int itemId)
                        {
                            if (await _itemUpdate.updateItemStatus(itemId, null, null, null, null, getStatus))
                            {
                                reservedItems.RemoveAt(reservedItems.Count - 1);
                                i--;
                            }
                        }
                    }
                }
                else if (quantity > reservedQuantity)
                {
                    int itemCount = quantity - reservedQuantity;
                    List<Item> availableItems = await _reserve.getItemsByStatus(getStatus);
                    List<Item> sortedItems = availableItems.OrderBy(x => { x.retrieveItemInfo().TryGetValue("ProductId", out var productIdObj); return productIdObj as int?; }).ThenByDescending(x => { x.retrieveItemInfo().TryGetValue("ExpiryDate", out var expiryDateObj); return expiryDateObj as DateTime?; }).ToList();
                    foreach (Item item in sortedItems)
                    {
                        if (item.retrieveItemInfo().TryGetValue("ProductId", out var iproductIdObj) && iproductIdObj is int iproductId && iproductId == productId && itemCount >= 1)
                        {
                            reservedItems.Add(item);
                            itemCount--;
                        }
                        else if (itemCount == 0)
                        {
                            break;
                        }
                    }
                    foreach (Item item in reservedItems)
                    {
                        try
                        {
                            if (item.retrieveItemInfo().TryGetValue("ItemId", out var itemIdObj) && itemIdObj is int itemId)
                            {
                                bool updatestatus = await _itemUpdate.updateItemStatus(itemId, reservationId, null, null, null, setStatus);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error updating item: {ex.Message}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Update Quantity Error");
                }
            }

            string result = await _reservationMapper.update(reservationId, productId, warehouseId, reservationDate, reservationPurpose, quantity, staffId);
            Console.WriteLine(result);
            return result;
        }

        public async Task<bool> ReturnReservedStockToinventory(Reservation reservation, int staffId)
        {
            Dictionary<string, object> reservationDict = reservation.GetReservationDetails();
            int reservationId = 0;
            string reservationPurpose = "";
            ItemStatus setStatus = ItemStatus.Available;
            List<Item> reservedItems = [];
            if (reservationDict.TryGetValue("ReservedItems", out var reservedItemsObj) && reservedItemsObj is List<Item> tempReservedItems) { reservedItems = tempReservedItems; }
            foreach (Item item in reservedItems)
            {
                try
                {
                    if (item.retrieveItemInfo().TryGetValue("ItemId", out var itemIdObj) && itemIdObj is int itemId)
                    {
                        bool updatestatus = await _itemUpdate.updateItemStatus(itemId, null, null, null, null, setStatus);
                        if (!updatestatus) { Console.WriteLine($"Error updating item: " + itemId); }
                        else
                        {
                            reservedItems.Remove(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating item: {ex.Message}");
                }
            }
            if (reservationDict.TryGetValue("ReservationId", out var reservationIdObj) && reservationIdObj is int tempReservationId) { reservationId = tempReservationId; }
            if (reservationDict.TryGetValue("ReservationPurpose", out var reservationPurposeObj) && reservationPurposeObj is string tempReservationPurpose) { reservationPurpose = tempReservationPurpose; }
            reservationPurpose += string.Join(" ", "[Returned]");
            string s = await UpdateReservationPurpose(reservationId, reservationPurpose, staffId);
            if (reservedItems.Count <= 0) { return true; } else { return false; }
        }

        public async Task<string> UpdateReservationPurpose(int reservationId, String reservationPurpose, int staffId)
        {
            Reservation reservation = await GetReservationById(reservationId);
            Dictionary<string, object> reservationDict = reservation.GetReservationDetails();
            int productId = 0, warehouseId = 0, reservedQuantity = 0;
            DateOnly reservationDate = DateOnly.FromDateTime(DateTime.Now);
            if (reservationDict.TryGetValue("ProductId", out var productIdObj) && productIdObj is int tempProductId) { productId = tempProductId; }
            if (reservationDict.TryGetValue("WarehouseId", out var warehouseIdObj) && warehouseIdObj is int tempWarehouseId) { warehouseId = tempWarehouseId; }
            if (reservationDict.TryGetValue("ReservationPurpose", out var reservationPurposeObj) && reservationPurposeObj is string tempReservationPurpose) { reservationPurpose = tempReservationPurpose; }
            if (reservationDict.TryGetValue("ReservedQuantity", out var reservedQuantityObj) && reservedQuantityObj is int tempReservedQuantity) { reservedQuantity = tempReservedQuantity; }
            string result = await _reservationMapper.update(reservationId, productId, warehouseId, reservationDate, reservationPurpose, reservedQuantity, staffId);
            Console.WriteLine(result);
            return result;
        }

        /*public async Task<String> ReserveStocks(int reservedQuantity, int warehouseId, int productId, String reservationPurpose, int status, int staffId, IConfiguration configuration)
        {
            Reservation reservation = new Reservation();
            List<Item> reservedItems = new List<Item>();
            ItemStatus getStatus = ItemStatus.Available;
            int itemCount = reservedQuantity;
            int reservationId = await _reservationMapper.GetNextId();
            reservation.ReservationId = reservationId;
            reservation.ProductId = productId;
            reservation.WarehouseId = warehouseId;
            reservation.ReservationDate = DateOnly.FromDateTime(DateTime.Now);
            reservation.ReservationPurpose = reservationPurpose;
            reservation.ReservedQuantity = reservedQuantity;
            reservation.StaffId = staffId;
            List<Item> availableItems = await _reserve.getItemsByStatus(getStatus);
            List<Item> sortedItems = availableItems.OrderBy(x => x.ProductId).ThenByDescending(x => x.ExpiryDate).ToList();
            foreach (Item item in sortedItems)
            {
                if (item.ProductId == reservation.ProductId && itemCount >= 1)
                {
                    reservedItems.Add(item);
                    itemCount--;
                }
                else if (itemCount == 0)
                {
                    break;
                }
            }
            ItemStatus setStatus = ItemStatus.Reserved;
            foreach (Item item in reservedItems)
            {
                try
                {
                    bool updatestatus = _itemUpdate.UpdateItemById(item.ItemId, item.ProductId, item.ExpiryDate, item.ReceiveDate, item.ManufactureDate,
                    item.SalePrice, item.BatchCode, item.WarehouseId, setStatus, reservationId, item.OrderId,
                    item.TransferId, item.ReturnId, configuration);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating item: {ex.Message}");
                }
            }
            reservation.ReservedItems = reservedItems;
            string result = await reservationMapper.insert(reservation.ReservationId, reservation.ProductId, reservation.WarehouseId,
                                     reservation.ReservationDate, reservation.ReservationPurpose,
                                     reservation.ReservedQuantity, reservation.StaffId);
            return result;
        }

        public async void UpdateReservationQuantity(int reservationId,  int quantity, int status, int staffId, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("CleanBrillantCompany:ConnectionString");
            ReservationMapper reservationMapper = new ReservationMapper(connectionString);
            Reservation reservation = await reservationMapper.findByReservationId(reservationId);
            reservation.ReservedQuantity = quantity;
            string result = await reservationMapper.update(reservation.ReservationId, reservation.ProductId, reservation.WarehouseId,
                                     reservation.ReservationDate, reservation.ReservationPurpose,
                                     quantity, staffId);
            Console.WriteLine(result);
        }

        public async void UpdateReservationPurpose(int reservationId, String reservationPurpose, int staffId, IConfiguration configuration)
        {
            Reservation reservation = new Reservation();
            string connectionString = configuration.GetConnectionString("CleanBrillantCompany:ConnectionString");
            ReservationMapper reservationMapper = new ReservationMapper(connectionString);
            string result = await reservationMapper.update(reservation.ReservationId, reservation.ProductId, reservation.WarehouseId,
                                     reservation.ReservationDate, reservationPurpose,
                                     reservation.ReservedQuantity, staffId);
            Console.WriteLine(result);
        }

        public void ReturnReservedStockToinventory(Reservation reservation, IConfiguration configuration)
        {   
            ItemStatus status = ItemStatus.Available;
            int reservationId = 0;
            foreach (Item item in reservation.ReservedItems){
                try
                {
                    bool updatestatus = _itemUpdate.UpdateItemById(item.ItemId, item.ProductId, item.ExpiryDate, item.ReceiveDate, item.ManufactureDate,
                    item.SalePrice, item.BatchCode, item.WarehouseId, status, reservationId, item.OrderId,
                    item.TransferId, item.ReturnId, configuration);
                    if (!updatestatus) { Console.WriteLine($"Error updating item: " + item.ItemId); }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating item: {ex.Message}");
                }
            }
        }*/
    }
}
