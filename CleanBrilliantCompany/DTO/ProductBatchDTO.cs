namespace CleanBrilliantCompany.DTO  // ✅ Correct way
{
    public class ProductBatchDTO
    {
        public int BatchCode { get; set; }
        public int ProductId { get; set; }
        public decimal BatchPrice { get; set; }  // Added
        public int BatchQuantity { get; set; }  
        public DateTime ExpiryDate { get; set; }
        public DateTime ReceiveDate { get; set; }
        public DateTime ManufactureDate { get; set; }
        public int ManufacturerId { get; set; }
    }

}