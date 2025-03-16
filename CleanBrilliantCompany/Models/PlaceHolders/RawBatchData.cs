// Temporary placeholder for the other team's BatchEntity
public class RawBatchData
{
    public int BatchCode { get; set; }
    public int ProductId { get; set; }
    public DateTime ExpiryDate { get; set; }
    public DateTime ReceiveDate { get; set; }
    public DateTime ManufactureDate { get; set; }
    public int Quantity { get; set; }
    public float SalesPrice { get; set; }

    // Constructor for easy instantiation
    public RawBatchData(int batchCode, int productId, DateTime expiryDate, DateTime receiveDate, DateTime manufactureDate, int quantity, float salesPrice)
    {
        BatchCode = batchCode;
        ProductId = productId;
        ExpiryDate = expiryDate;
        ReceiveDate = receiveDate;
        ManufactureDate = manufactureDate;
        Quantity = quantity;
        SalesPrice = salesPrice;
    }
}
