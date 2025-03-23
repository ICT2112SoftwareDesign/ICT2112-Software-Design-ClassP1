public class FakeProductInterface {

    private Dictionary <int , RawProductData> fakeProducts;

    public FakeProductInterface()
    {
        fakeProducts = new Dictionary<int, RawProductData>
        {
            {
                990, new RawProductData(
                    productID: 990,
                    productName: "Apple",
                    productCategory: "Fruit",
                    costPrice: 0.5,
                    weight: 0.2,
                    quantity: 100)
            },
            {
                2, new RawProductData(
                    productID: 2,
                    productName: "Banana",
                    productCategory: "Fruit",
                    costPrice: 0.3,
                    weight: 0.1,
                    quantity: 200)
            },
            {
                909, new RawProductData(
                    productID: 909,
                    productName: "Orange",
                    productCategory: "Fruit",
                    costPrice: 0.4,
                    weight: 0.15,
                    quantity: 150)
            },
            {
                23, new RawProductData(
                    productID: 23,
                    productName: "Pineapple",
                    productCategory: "Fruit",
                    costPrice: 1.5,
                    weight: 1.5,
                    quantity: 50)
            },
            {
                14, new RawProductData(
                    productID: 14,
                    productName: "Mango",
                    productCategory: "Fruit",
                    costPrice: 1.0,
                    weight: 0.5,
                    quantity: 80)
            }
        };
    }

    public RawProductData? getProductDetails(int productID)
    {
        if (fakeProducts.ContainsKey(productID))
        {
            return fakeProducts[productID]; 
        }
        return null;
    } 
}