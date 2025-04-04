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

            return View("~/Views/Product/Product.cshtml", productInfo);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(string productName, string productCategory, float productCost, 
        int manufacturerId, float productWeight, int volume, float toxicityPercentage, int carbonFootprint, bool isLiquid)
        {
            try
            {

                // Input Validation
                if (string.IsNullOrWhiteSpace(productName) || string.IsNullOrWhiteSpace(productCategory))
                {
                    TempData["ErrorMessage"] = "Product name and category cannot be empty.";
                    return RedirectToAction("Index");
                }

                if (manufacturerId <= 0)
                {
                    TempData["ErrorMessage"] = "Invalid manufacturer ID.";
                    return RedirectToAction("Index");
                }

                if (productCost <= 0)
                {
                    TempData["ErrorMessage"] = "Product cost must be greater than 0.";
                    return RedirectToAction("Index");
                }

                if (productWeight <= 0)
                {
                    TempData["ErrorMessage"] = "Product weight must be greater than 0.";
                    return RedirectToAction("Index");
                }

                if (volume < 0)
                {
                    TempData["ErrorMessage"] = "Volume cannot be negative.";
                    return RedirectToAction("Index");
                }

                if (toxicityPercentage < 0)
                {
                    TempData["ErrorMessage"] = "Toxicity percentage cannot be negative.";
                    return RedirectToAction("Index");
                }

                if (carbonFootprint < 0)
                {
                    TempData["ErrorMessage"] = "Carbon footprint cannot be negative.";
                    return RedirectToAction("Index");
                }

                int productId = _productControl.createProduct(productName, productCategory, productCost, manufacturerId, 
                                                          productWeight, 0, volume, toxicityPercentage, carbonFootprint, isLiquid);


                if (productId != -1) // Successful creation
                {
                    TempData["SuccessMessage"] = $"Product '{productName}' created successfully with ID: {productId}.";
                }
                else // Failure
                {
                    TempData["ErrorMessage"] = $"Failed to create product '{productName}'.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error creating product: {ex.Message}";
            }
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

            List<Dictionary<string, object>> manufacturersInfo = new List<Dictionary<string, object>>();
            List<ProductManufacturer> manufacturers = _productControl.getAllProductManufacturer();

            foreach (var manufacturer in manufacturers)
            {
                manufacturersInfo.Add(manufacturer.retrieveProductManufacturerInfo());
            }

            ViewBag.Manufacturers = manufacturersInfo;

            return View("~/Views/Product/Product.cshtml", productInfo);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateProduct(int productId, string productName, string productCategory, float productCost, 
        float productWeight, int quantity, int volumeOrZero, float toxicityPercentage, int carbonFootprint, bool isLiquid)
        {
            try
            {
                // Input Validation
                if (string.IsNullOrWhiteSpace(productName) || string.IsNullOrWhiteSpace(productCategory))
                {
                    TempData["ErrorMessage"] = "Product name and category cannot be empty.";
                    return RedirectToAction("Index");
                }

                if (productCost <= 0)
                {
                    TempData["ErrorMessage"] = "Product cost must be greater than 0.";
                    return RedirectToAction("Index");
                }

                if (productWeight <= 0)
                {
                    TempData["ErrorMessage"] = "Product weight must be greater than 0.";
                    return RedirectToAction("Index");
                }

                if (volumeOrZero < 0)
                {
                    TempData["ErrorMessage"] = "Volume cannot be negative.";
                    return RedirectToAction("Index");
                }

                if (toxicityPercentage < 0)
                {
                    TempData["ErrorMessage"] = "Toxicity percentage cannot be negative.";
                    return RedirectToAction("Index");
                }

                if (carbonFootprint < 0)
                {
                    TempData["ErrorMessage"] = "Carbon footprint cannot be negative.";
                    return RedirectToAction("Index");
                }

                // Collect the manufacturerId
                Product product = _productControl.getProductDetails(productId);
                List<Dictionary<string, object>> productInfo = new List<Dictionary<string, object>>();
                productInfo.Add(product.retrieveProductInfo());
                int oldManufacturerId = Convert.ToInt32(productInfo[0]["ManufacturerId"]);

                // Based on isLiquid update volume
                string productState = isLiquid ? "1" : "0";
                int volume = isLiquid ? volumeOrZero : 0;
                bool isUpdated = _productControl.updateProduct(productId, productName, productCategory, 
                                            productCost, oldManufacturerId, productWeight, 
                                            quantity, volume, toxicityPercentage, 
                                            carbonFootprint, productState);

                // Check if the update was successful
                if (isUpdated)
                {
                    TempData["SuccessMessage"] = $"Product '{productName}' (ID: {productId}) updated successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = $"Error updating product '{productName}' (ID: {productId}).";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Exception: {ex.Message}";
            }
            return RedirectToAction("Index");
        }

        // Reorder Request ** SIMULTATION **
        public async Task<IActionResult> ReorderRequest() 
        {
            _productControl.processReorderRequest();
            return RedirectToAction("Index");
        }
    }
}