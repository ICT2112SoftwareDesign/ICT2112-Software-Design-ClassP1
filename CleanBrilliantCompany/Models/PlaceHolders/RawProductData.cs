public class RawProductData {
    public int productID { get; set; } 
    public string productName { get; set; } = "";
    public string productCategory { get; set; }  = ""; 
    public double costPrice { get; set; } 
    public double weight { get; set; } 
    public int quantity { get; set; }


    public RawProductData (int productID, string productName, string productCategory, double costPrice, double weight, int quantity) {
        this.productID = productID;
        this.productName = productName;
        this.productCategory = productCategory;
        this.costPrice = costPrice;
        this.weight = weight;
        this.quantity = quantity;
    }
}