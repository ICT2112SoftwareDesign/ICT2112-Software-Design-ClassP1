using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Data;

namespace CleanBrilliantCompany.Models.CalculatorImplementation
{
    public class CalculateProductCFImpl : IProductCFCalculator
    {
        private readonly IProduct _IProduct;

        public float CalculateCarbonFootprint(float vol, float tox, int productId)
        {
            ProductDBStub stub = new ProductDBStub(_IProduct);
            Product product = stub.GetProductDetails(productId);
            // Insert product name and category e.g. into CF record DB
            // product.ProductName, product.ProductCategory insert

            return vol * tox;
        }
    }
}
