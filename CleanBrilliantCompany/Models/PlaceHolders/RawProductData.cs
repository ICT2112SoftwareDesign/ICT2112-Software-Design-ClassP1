public class RawProductData {
    public int productId { get; set; } 

    public string productName { get; set; } 
    
    public string productCategory { get; set; }

    public double productCost { get; set; } 

    public int manufacturerId { get; set; } 

    public double productWeight { get; set; } 

    public int quantity { get; set; } 
    public int volume { get; set; } 

    public double toxicityPercentage { get; set; } 

    public int carbonFootprint { get; set; }

    public string productState { get; set; } 


    public RawProductData ( int productId, string productName, string productCategory, double productCost, int manufacturerId, double productWeight, int quantity, int volume, double toxicityPercentage, int carbonFootprint, string productState ) {
        this.productId = productId;
        this.productName = productName;
        this.productCategory = productCategory;
        this.productCost = productCost;
        this.manufacturerId = manufacturerId;
        this.productWeight = productWeight;
        this.quantity = quantity;
        this.volume = volume;
        this.toxicityPercentage = toxicityPercentage;
        this.carbonFootprint = carbonFootprint;
        this.productState = productState;
    }
}