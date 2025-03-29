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

        /*
        public int ReservationId { get; set; }
        public int ProductId { get; set; }
        public int WarehouseId { get; set; }
        public DateOnly ReservationDate { get; set; }
        public string ReservationPurpose { get; set; }
        public int ReservedQuantity { get; set; } = 0;
        public int StaffId { get; set; }
        public List<Item> ReservedItems { get; set; }
        */

        // Private fields
        private int ReservationId;
        private int ProductId;
        private int WarehouseId;
        private DateOnly ReservationDate;
        private string ReservationPurpose;
        private int ReservedQuantity;
        private int StaffId;
        private List<Item>? ReservedItems;

        public Reservation(int reservationId, int productId, int warehouseId, DateOnly reservationDate,
                           string reservationPurpose, int reservedQuantity, int staffId , List<Item>? reservedItems)
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

        public static Reservation CreateReservation(int reservationId, int productId, int warehouseId, DateOnly reservationDate,
                           string reservationPurpose, int reservedQuantity, int staffId, List<Item>? reservedItems)
        {
            return new Reservation(reservationId, productId, warehouseId, reservationDate, reservationPurpose, reservedQuantity, staffId, reservedItems);
        }

        public void UpdateReservation(int reservationId, int productId, int warehouseId, DateOnly reservationDate,
                           string reservationPurpose, int reservedQuantity, int staffId, List<Item>? reservedItems)
        {
            ReservationId = reservationId;
            ProductId = productId;
            WarehouseId = warehouseId;
            ReservationDate = reservationDate;
            ReservationPurpose = reservationPurpose;
            ReservedQuantity = reservedQuantity;
            StaffId = staffId;
            ReservedItems = reservedItems ?? ReservedItems;
        }

        public void InsertItems(int? reservationId,  List<Item>? reservedItems)
        {
            ReservationId = reservationId ?? ReservationId;
            ReservedItems = reservedItems ?? ReservedItems;
        }

        public Dictionary<string, object> GetReservationDetails()
        {
            return new Dictionary<string, object>
            {
                { "ReservationId", ReservationId },
                { "ProductId", ProductId },
                { "WarehouseId", WarehouseId },
                { "ReservationDate", ReservationDate },
                { "ReservationPurpose", ReservationPurpose },
                { "ReservedQuantity", ReservedQuantity },
                { "StaffId", StaffId },
                { "ReservedItems" , ReservedItems }
            };
        }

        private int GetReservationId() => ReservationId;
        private int GetProductId() => ProductId;
        private int GetWarehouseId() => WarehouseId;
        private DateOnly GetReservationDate() => ReservationDate;
        private string GetReservationPurpose() => ReservationPurpose;
        private int GetReservedQuantity() => ReservedQuantity;
        private int GetStaffId() => StaffId;
        private List<Item>? GetReservedItems() => ReservedItems;

        private void SetReservationId(int reservationId) => ReservationId = reservationId;
        private void SetProductId(int productId) => ProductId = productId;
        private void SetWarehouseId(int warehouseId) => WarehouseId = warehouseId;
        private void SetReservationDate(DateOnly reservationDate) => ReservationDate = reservationDate;
        private void SetReservationPurpose(string reservationPurpose) => ReservationPurpose = reservationPurpose;
        private void SetReservationQuantity(int reservationQuantity) => ReservedQuantity = reservationQuantity;
        private void SetStaffId(int staffId) => StaffId = staffId;
        private void SetReservedItems(List<Item>? reservedItems) => ReservedItems = reservedItems;

        public Reservation() { }
    }

}