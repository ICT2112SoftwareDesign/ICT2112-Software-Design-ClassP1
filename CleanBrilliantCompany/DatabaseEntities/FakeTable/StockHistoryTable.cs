using System.ComponentModel.DataAnnotations;

public class StockHistoryTable 
{
    [Key]
    public int stockId { get; set; } 

    public int batchCode { get; set; } 

    public DateOnly stockTakeDate { get; set; } 

    public int quantity { get; set; } 

    public DateTime recordedDate {get; set; } 
}