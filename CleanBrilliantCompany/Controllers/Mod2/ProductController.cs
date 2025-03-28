using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models.Control;
using CleanBrilliantCompany.Models.Entity;
using Microsoft.Extensions.Configuration;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductControl _productControl;

        public ProductController(ProductControl productControl)
        {
            _productControl = productControl;
        }

        public async Task<IActionResult> Index()
        {
            List<Dictionary<string, object>> productInfo = new List<Dictionary<string, object>>();

            List<Product> products = _productControl.getAllProducts();

            foreach (var product in products)
            {
                productInfo.Add(product.retrieveProductInfo());
            }

            List<Dictionary<string, object>> manufacturersInfo = new List<Dictionary<string, object>>();
            List<ProductManufacturer> manufacturers = _productControl.getAllProductManufacturer();

            foreach (var manufacturer in manufacturers)
            {
                manufacturersInfo.Add(manufacturer.retrieveProductManufacturerInfo());

            }

            ViewBag.Manufacturers = manufacturersInfo;

            return View("~/Views/Product/Index.cshtml", productInfo);
        }

        // public async Task<IActionResult> displayProducts()
        // {
        //     List<Dictionary<string, object>> productInfo = new List<Dictionary<string, object>>();

        //     List<Product> products = _productControl.getAllProducts();

        //     foreach (var product in products)
        //     {
        //         productInfo.Add(product.retrieveProductInfo());
        //     }

        //     return View("~/Views/Product/Index.cshtml", productInfo);
        // }

        // [HttpPost]
        // public async Task<IActionResult> CreateProduct(string productName, string productCategory, float productCost, 
        // int manufacturerId, float weight, int volume, float toxicityPercentage, int carbonFootprint, string productState)
        // {
        //     // Create the product without quantity, to represent just a new prod into the list
        //     int quantity = 0; // When creating a product should be default to 0 quantity, no items
        //     _productControl.createProduct(productName, productCategory, 
        //                                 productCost, manufacturerId, weight, 
        //                                 quantity, volume, toxicityPercentage, 
        //                                 carbonFootprint, productState);

        //     return RedirectToAction("Index");
        // }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(string productName, string productCategory, float productCost, 
        int manufacturerId, float weight, int volume, float toxicityPercentage, int carbonFootprint, bool isLiquid)
        {

            int quantity = 0; // New product, so default quantity is 0
            if (isLiquid)
            {
                _productControl.CreateLiquidProduct(productName, productCategory, productCost,
                                                    manufacturerId, weight, quantity, volume,
                                                    toxicityPercentage, carbonFootprint);
            }
            else
            {
                _productControl.CreateSolidProduct(productName, productCategory, productCost,
                                                manufacturerId, weight, quantity,
                                                toxicityPercentage, carbonFootprint);
            }

            return RedirectToAction("Index");
        }

        // Might remove
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            _productControl.deleteProduct(productId);

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> FetchProduct(int productId)
        {
            List<Dictionary<string, object>> productInfo = new List<Dictionary<string, object>>();
            Product product = _productControl.getProductDetails(productId);

            if (product != null)
            {
                productInfo.Add(product.retrieveProductInfo());
            }
            return View("~/Views/Product/TestProduct.cshtml", productInfo);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(int productId, string productName, string productCategory, float productCost, 
        int manufacturerId, float productWeight, int quantity, int volume, float toxicityPercentage, int carbonFootprint, string productState)
        {
            _productControl.updateProduct(productId, productName, productCategory, 
                                        productCost, manufacturerId, productWeight, 
                                        quantity, volume, toxicityPercentage, 
                                        carbonFootprint, productState);;
            return RedirectToAction("Index");
        }

        // Product Batch
        public async Task<IActionResult> displayProductBatch()
        {

            List<Dictionary<string, object>> batchInfo = new List<Dictionary<string, object>>();
            List<ProductBatch> productBatches = _productControl.getAllProductBatch();

            foreach (var productBatch in productBatches)
            {
                batchInfo.Add(productBatch.retrieveProductBatchInfo());
            }

            return View("~/Views/Product/ProductBatch.cshtml", batchInfo);
        }

        [HttpPost]
        public async Task<IActionResult> FetchBatch(int batchCode)
        {
            List<Dictionary<string, object>> batchInfo = new List<Dictionary<string, object>>();
            ProductBatch batch = _productControl.getBatchDetails(batchCode);

            if (batch != null)
            {
                batchInfo.Add(batch.retrieveProductBatchInfo());
            }
            return View("~/Views/Product/ProductBatch.cshtml", batchInfo);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductBatch(int productId, DateTime expiryDate, 
            DateTime receiveDate, DateTime manufactureDate, int quantity, int batchCost)
        {
            _productControl.createProductBatch(productId, expiryDate, 
                    receiveDate, manufactureDate, quantity, batchCost);

            // Add items

            return RedirectToAction("displayProductBatch");
        }

        // Reorder Request
        public async Task<IActionResult> ReorderRequest() 
        {
            _productControl.processReorderRequest();
            return RedirectToAction("Index");
        }
    }
}