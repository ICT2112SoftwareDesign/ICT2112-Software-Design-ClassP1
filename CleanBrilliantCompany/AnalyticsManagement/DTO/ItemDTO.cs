namespace CleanBrilliantCompany.DTO  // ✅ Correct way
{
    public class ItemDTO
    {
        public int ItemId { get; set; }
        public int ProductId { get; set; }
        public decimal SalePrice { get; set; }
        public int BatchCode { get; set; }
        public int WarehouseId { get; set; }
        public string? ItemStatus { get; set; }  // Assuming it's a string (e.g., "Available", "Sold", etc.)
        public int? ReservationId { get; set; }  // Nullable in case an item is not reserved
        public int? OrderId { get; set; }  // Nullable if not yet ordered
        public int? TransferId { get; set; }  // Nullable if not transferred
        public int? ReturnId { get; set; }  // Nullable if not returned
    }

}