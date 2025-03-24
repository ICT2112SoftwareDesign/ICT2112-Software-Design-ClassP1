using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Data;

namespace CleanBrilliantCompany.Models.CalculatorImplementation
{
    public class CalculateProductCFImpl : IProductCFCalculator
    {
        private readonly IProduct _IProduct; // for retrieving product from Mod 2's product table
        private readonly IProductCFManagement _productCFManagement; // for inserting new product CF into our product records table

        public CalculateProductCFImpl(IProduct iProduct, IProductCFManagement productCFManagement)
        {
            _IProduct = iProduct;
            _productCFManagement = productCFManagement;
        }

        public float CalculateCarbonFootprint(float vol, float tox, int productId)
        {
            ProductDBStub stub = new ProductDBStub();
            Product product = stub.GetProductDetails(productId);

            float carbonEmission = vol * tox;
            string ecoStatus = carbonEmission >= 250 ? "Not Eco-Friendly" : "Eco-Friendly";

            // Insert product data into CF record DB
            _productCFManagement.addProductCF(
                productId,
                product.ProductName,
                product.ProductCategory,
                carbonEmission, // carbon emission
                ecoStatus,
                DateTime.Now // dateCreated
            );

            return carbonEmission;
        }
    }
}
