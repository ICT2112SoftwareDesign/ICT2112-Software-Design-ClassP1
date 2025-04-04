public class ReorderRequestSample
{
    public int ReorderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public int ManufacturerId { get; set; }
    public DateTime ExpectedDeliveryDate { get; set; }
    public string Status { get; set; }
    public int DefectQuantity { get; set; }
}