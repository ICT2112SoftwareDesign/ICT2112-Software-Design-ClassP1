using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Controllers
{
    public class ProductInputController : ApplicationController
    {
        private readonly ILogger<CustomerPageController> _logger;
        private readonly CustomerManagement _customerManagement;
        private readonly IProduct _productService;

        private readonly ReviewManagement _reviewManagement;


        public ProductInputController(
            ILogger<CustomerPageController> logger,
            CustomerManagement customerManagement,
            IProduct productService,
            ReviewManagement reviewManagement,
            IHttpContextAccessor httpContextAccessor
            ) : base(customerManagement, httpContextAccessor)
        {
            _logger = logger;
            _customerManagement = customerManagement;
            _productService = productService;
            _reviewManagement = reviewManagement;
        }       

        public IActionResult viewProducts(string query = "", string filters = "All", string sortOrder = "asc")
        {
            List<Product> products = _productService.getAllProducts();

            // Apply search filter
            if (!string.IsNullOrEmpty(query))
            {
                products = products.Where(p => p.GetProductDetails()["ProductName"].ToString().Contains(query, System.StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Apply category filter
            if (filters != "All")
            {
                products = products.Where(p => p.GetProductDetails()["Category"].ToString() == filters).ToList();
            }

            // Apply sorting
            products = sortOrder == "asc"
                ? products.OrderBy(p => float.Parse(p.GetProductDetails()["CostPrice"].ToString())).ToList()
                : products.OrderByDescending(p => float.Parse(p.GetProductDetails()["CostPrice"].ToString())).ToList();

            // Convert to a list of dictionaries
            var productDetails = products.Select(product => product.GetProductDetails()).ToList();

            return View("~/Views/Products/Index.cshtml", productDetails);
        }

        public List<Product> filterProducts(List<string> categories)
        {
            var allProducts = _productService.getAllProducts();
            return allProducts.FindAll(p => categories.Contains(p.GetProductDetails()["Category"].ToString()));
        }

        [HttpGet]
        public IActionResult viewProductDetails(int productId)
        {
            var product = _productService.getProductDetails(productId);
            if (product == null)
            {
                TempData["Error"] = "Product not found.";
                return RedirectToAction("GetAllProducts");
            }

            var productDetails = product.GetProductDetails();

            var reviews = _reviewManagement.ViewReviewsByProduct(productId);

            ViewBag.ProductReviews = reviews;
            ViewBag.ProductId = productId;
            return View("~/Views/Products/ProductDetails.cshtml", productDetails);
        }

   }
}