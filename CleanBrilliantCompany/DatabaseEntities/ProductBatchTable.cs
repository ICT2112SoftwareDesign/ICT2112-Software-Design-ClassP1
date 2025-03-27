using System.ComponentModel.DataAnnotations;

public class ProductBatchTable
{
    [Key] 
    public int BatchCode { get; set; }
    public int ProductId { get; set; }
    public DateTime ExpiryDate { get; set; }
    public DateTime ReceiveDate { get; set; }
    public DateTime ManufactureDate { get; set; }
    public int Quantity { get; set; }
    public double BatchCost { get; set; }

}
