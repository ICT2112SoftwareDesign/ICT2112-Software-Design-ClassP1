namespace CleanBrilliantCompany.DTO
{
    public class ReorderRequestDTO
    {
        private int reorderId;
        private int productId;
        private int quantity;
        private int manufacturerId;
        private DateTime expectedDeliveryDate;
        private string status;
        private int defectQuantity;

        public int ReorderId { get { return reorderId; } set { reorderId = value; } }
        public int ProductId { get { return productId; } set { productId = value; } }
        public int Quantity { get { return quantity; } set { quantity = value; } }
        public int ManufacturerId { get { return manufacturerId; } set { manufacturerId = value; } }
        public DateTime ExpectedDeliveryDate { get { return expectedDeliveryDate; } set { expectedDeliveryDate = value; } }
        public string Status { get { return status; } set { status = value; } }
        public int DefectQuantity { get { return defectQuantity; } set { defectQuantity = value; } }

        public ReorderRequestDTO(int reorderId, int productId, int quantity, int manufacturerId, DateTime expectedDeliveryDate, string status, int defectQuantity)
        {
            this.reorderId = reorderId;
            this.productId = productId;
            this.quantity = quantity;
            this.manufacturerId = manufacturerId;
            this.expectedDeliveryDate = expectedDeliveryDate;
            this.status = status;
            this.defectQuantity = defectQuantity;
        }
    }
}