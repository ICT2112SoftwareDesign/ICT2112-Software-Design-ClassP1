using System.ComponentModel.DataAnnotations;
public class ItemTable
{
    [Key] 
    public int ItemId { get; set; }
    public int ProductId { get; set; }
    public double SalePrice { get; set; }
    public int BatchCode { get; set; }
    public int WarehouseId { get; set; }
    public string? ItemStatus { get; set; }
    public int? ReservationId { get; set; }
    public int? OrderId { get; set; }
    public int? TransferId { get; set; }
    public int? ReturnId { get; set; }
}
