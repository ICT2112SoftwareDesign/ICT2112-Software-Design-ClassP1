using System.ComponentModel.DataAnnotations;

public class BatchTable 
{
    [Key]
    public int batchCode { get; set; } 

    public int productId { get; set; } 

    public DateTime expiryDate { get; set; } 

    public DateTime receiveDate { get; set; } 

    public DateTime manufactureDate { get; set; } 

    public int quantity { get; set; } 

    public double batchCost { get; set; } 
}