using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Mappers;
using CleanBrilliantCompany.Models.Entity;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace CleanBrilliantCompany.Models.Control
{
    public class ReservationControl
    {
        private readonly IItem _item;
        private readonly IIItemUpdate _itemUpdate;
        private readonly IReserve _reserve;
        private readonly IStaffDetails _staffDetails;
        private readonly ReservationMapper _reservationMapper;

        public ReservationControl(IItem item, IIItemUpdate itemUpdate, IReserve reserve, ReservationMapper reservationMapper)
        {
            _item = item;
            _itemUpdate = itemUpdate;
            _reserve = reserve;
            _reservationMapper = reservationMapper;
        }

        public async Task<String> ReserveStock(int reservedQuantity, int warehouseId, int productId, String reservationPurpose, int status, int staffId, IConfiguration configuration)
        {
            Reservation reservation = new Reservation();
            List<Item> reservedItems = new List<Item>();
            string connectionString = configuration.GetConnectionString("CleanBrillantCompany:ConnectionString");
            ReservationMapper reservationMapper = new ReservationMapper(connectionString);
            Status getStatus = Status.Available;
            int itemCount = reservedQuantity;
            int reservationId = await reservationMapper.getNextId();
            reservation.ReservationId = reservationId;
            reservation.ProductId = productId;
            reservation.WarehouseId = warehouseId;
            reservation.ReservationDate = DateOnly.FromDateTime(DateTime.Now);
            reservation.ReservationPurpose = reservationPurpose;
            reservation.ReservedQuantity = reservedQuantity;
            reservation.StaffId = staffId;
            List<Item> availableItems = _reserve.GetItemsByStatus(getStatus, configuration);
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
            Status setStatus = Status.Reserved;
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
            Status status = Status.Available;
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
        }
    }
}
