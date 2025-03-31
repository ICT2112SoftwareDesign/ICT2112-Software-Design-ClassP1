using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Data;
using System.Globalization;

namespace CleanBrilliantCompany.DomainControl
{
    public class ProductControl : IProductQuery, IProduct
    {
        private readonly ProductMapper _productMapper;

        public ProductControl(ProductMapper productMapper){
            _productMapper = productMapper;
        }

        // Product
        public Product getProductDetails(int productId)
        {
            return  _productMapper.findByProductId(productId);
        }


        public List<Product> getAllProducts()
        {
            return  _productMapper.findAllProducts();
        }

        public void createProduct(string productName, string category, float productCost, 
        int manufacturerId, float weight, int quantity, int volume, float toxicityPercentage, int carbonFootprint, string productState)
        {
            _productMapper.insert(productName, category, productCost, 
                                    manufacturerId, weight, quantity, volume, toxicityPercentage, carbonFootprint, productState);
        }
    }
}