namespace CleanBrilliantCompany.Models.Entity
{
    public class Reservation
    {
        /*
        - reservationId: Int
        - productId: Int
        - warehouseId: Int
        - reservationDate: Date
        - reservationPurpose: String
        - reservedQuantity: Int
        - reservedItems: List<Item>
        */


        public int ReservationId { get; set; }
        public int ProductId { get; set; }
        public int WarehouseId { get; set; }
        public DateOnly ReservationDate { get; set; }
        public string ReservationPurpose { get; set; }
        public int ReservedQuantity { get; set; } = 0;
        public int StaffId { get; set; }
        public List<Item> ReservedItems { get; set; }


        public Reservation(int reservationId, int productId, int warehouseId, DateOnly reservationDate,
                           string reservationPurpose, int reservedQuantity, int staffId , List<Item> reservedItems)
        {
            ReservationId = reservationId;
            ProductId = productId;
            WarehouseId = warehouseId;
            ReservationDate = reservationDate;
            ReservationPurpose = reservationPurpose;
            ReservedQuantity = reservedQuantity;
            StaffId = staffId;
            ReservedItems = reservedItems;
        }

        public Reservation() { }
    }

}