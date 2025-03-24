public class FakeProductInterface {

    private readonly SimulatedDbContext _context;   

    public FakeProductInterface(SimulatedDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context)); 

    }

    public RawProductData? getProductDetails(int productID)
    {
        var product = _context.Products.FirstOrDefault(p => p.productId == productID);
        if (product == null) return null;

        return new RawProductData(
            productId: product.productId,
            productName: product.productName,
            productCategory: product.productCategory,
            productCost: product.productCost,
            manufacturerId: product.manufacturerId,
            productWeight: product.productWeight,
            quantity: product.quantity,
            volume: product.volume,
            toxicityPercentage: product.toxicityPercentage,
            carbonFootprint: product.carbonFootprint,
            productState: product.productState
        ); 
    } 
}