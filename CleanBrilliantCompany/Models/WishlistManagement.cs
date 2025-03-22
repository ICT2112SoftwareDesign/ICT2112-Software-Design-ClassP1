using System.Collections.Generic;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models
{
    public class WishlistManagement : IWishlistManagement
    {
        private WishlistRDM wishlistRDM = new WishlistRDM();

        private readonly IWishlistDatabase _wishlistDatabase;
        private readonly IProduct _productService;

        public WishlistManagement(IWishlistDatabase wishlistDatabase, IProduct productService)
        {
            _wishlistDatabase = wishlistDatabase;
            _productService = productService;
        }

    // Convert string of comma-separated product IDs to List<int>
    private List<int> ConvertStringToProductIdList(string productIdsString)
    {
        var productIds = new List<int>();
        
        if (!string.IsNullOrEmpty(productIdsString))
        {
            string[] productIdStrings = productIdsString.Split(',');
            foreach (var idString in productIdStrings)
            {
                if (int.TryParse(idString.Trim(), out int productId))
                {
                    productIds.Add(productId);
                }
            }
        }
        
        return productIds;
    }
    
    // Convert List<int> to comma-separated string
    private string ConvertProductIdListToString(List<int> productIds)
    {
        return string.Join(",", productIds);
    }

    public bool addToWishlist(int customerId, int productId)
    {
        // Get current wishlist
        string productIdsString = _wishlistDatabase.getWishlistProductIdsString(customerId);
        List<int> wishlist = ConvertStringToProductIdList(productIdsString);
        
        // Check if product already exists in wishlist
        if (wishlist.Contains(productId))
        {
            return false; // Product already in wishlist
        }
        
        // Add product to wishlist
        wishlist.Add(productId);
        
        // Convert back to string and save
        string updatedProductIdsString = ConvertProductIdListToString(wishlist);
        return _wishlistDatabase.saveWishlistProductIdsString(customerId, updatedProductIdsString);
    }

    public bool removeFromWishlist(int customerId, int productId)
    {
        string productIdsString = _wishlistDatabase.getWishlistProductIdsString(customerId);
        List<int> wishlist = ConvertStringToProductIdList(productIdsString);
        
        if (!wishlist.Contains(productId))
        {
            return false; // Product not in wishlist
        }
        
        wishlist.Remove(productId);
        
        string updatedProductIdsString = ConvertProductIdListToString(wishlist);
        return _wishlistDatabase.saveWishlistProductIdsString(customerId, updatedProductIdsString);
    }

    public List<int> viewWishlist(int customerId)
    {
        string productIdsString = _wishlistDatabase.getWishlistProductIdsString(customerId);
        return ConvertStringToProductIdList(productIdsString);
    }

    public Dictionary<int, Dictionary<string, object>> GetWishlistProductDetails(int customerId)
    {
        List<int> wishlist = viewWishlist(customerId);
        var products = new Dictionary<int, Dictionary<string, object>>();
        
        foreach (var productId in wishlist)
        {
            var product = _productService.getProductDetails(productId);
            if (product != null)
            {
                products.Add(productId, product.GetProductDetails());
            }
        }
        
        return products;
    }
    
    // Method to check if a wishlist exists for a customer
    // If not, create an empty one
    public void EnsureWishlistExists(int customerId)
    {
        if (!_wishlistDatabase.customerWishlistExists(customerId))
        {
            _wishlistDatabase.saveWishlistProductIdsString(customerId, "");
        }
    }
    }
}