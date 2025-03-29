namespace CleanBrilliantCompany.DatabaseEntities
{


using System.ComponentModel.DataAnnotations;

public class ProductTable
{
    [Key]     
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductCategory { get; set; }
    public double ProductCost { get; set; }
    public int ManufacturerId { get; set; }
    public double ProductWeight { get; set; }
    public int Quantity { get; set; }
    public int Volume { get; set; }
    public double ToxicityPercentage { get; set; }
    public int CarbonFootprint { get; set; }
    public string ProductState { get; set; }
}

}