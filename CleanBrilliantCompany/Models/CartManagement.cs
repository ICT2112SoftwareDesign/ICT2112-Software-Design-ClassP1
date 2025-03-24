using System.Collections.Generic;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;


// Manages the shopping cart for customers, including adding, updating, and removing products.\
// Integrates with the Singleton `CartRDM` to ensure a single cart instance per customer.
namespace CleanBrilliantCompany.Models
{
    public class CartManagement : ICartManagement
    {
        private ICartDatabase cartDatabase;
        private IProduct productService;
  

        public CartManagement(ICartDatabase cartDatabase, IProduct productService)
        {
            this.cartDatabase = cartDatabase;
            this.productService = productService;
            
        }

        // Adds a product to the cart
        public bool addToCart(int customerID, int productId, int quantity)
        {
            var productDetails = productService.getProductDetails(productId);
            if (productDetails != null)
            {
                // Get the Singleton instance of CartRDM for the customer
                var cartRDM = CartRDM.GetInstance(customerID);

                // Add the product ID and quantity to the in-memory cart (CartRDM)
                cartRDM.addProduct(productId, quantity);

                // Update the database with the new cart content
                var success = cartDatabase.addCart(customerID, cartRDM.retrieveProductsInCart());
                return success;
            }
            return false;
        }

        // Updates the quantity of a product in the cart
        public bool updateQuantity(int customerID, int productId, int quantity)
        {
            // Get the Singleton instance of CartRDM for the customer
            var cartRDM = CartRDM.GetInstance(customerID);

            // Load the cart from the database into the in-memory cart
            if (cartDatabase.getCart(customerID, out var cartData))
            {
                cartRDM.loadCart(cartData);
            }
            else
            {
                return false;
            }

            // Check if the product exists in the in-memory cart
            if (cartRDM.hasProduct(productId))
            {
                // Update the in-memory cart
                cartRDM.updateProductQuantity(productId, quantity);

                // Update the database with the new cart content
                var success = cartDatabase.updateCart(customerID, cartRDM.retrieveProductsInCart());
                return success;
            }
            return false;
        }

        // Removes a product from the cart
        public bool removeFromCart(int customerID, int productId)
        {
            // Get the Singleton instance of CartRDM for the customer
            var cartRDM = CartRDM.GetInstance(customerID);

            // Load the cart from the database into the in-memory cart
            if (cartDatabase.getCart(customerID, out var cartData))
            {
                cartRDM.loadCart(cartData);
            }
            else
            {
                return false;
            }

            // Check if the product exists in the in-memory cart
            if (cartRDM.hasProduct(productId))
            {
                // Remove the product from the in-memory cart
                cartRDM.removeProduct(productId);

                // Update the database with the new cart content
                var success = cartDatabase.removeFromCart(customerID, productId);
                return success;
            }

            return false;
        }

        // Clears the cart after an order is placed
        public bool clearCart(int customerID)
        {
            // Get the Singleton instance of CartRDM for the customer
            var cartRDM = CartRDM.GetInstance(customerID);

            // Clear the in-memory cart
            cartRDM.loadCart(new Dictionary<int, int>());

            // Call the clearCart method in the cartDatabase
            var success = cartDatabase.clearCart(customerID);
            return success;
        }

        // Retrieves the cart for a specific customer
        public Dictionary<int, int> viewCart(int customerID)
        {
            // Get the Singleton instance of CartRDM for the customer
            var cartRDM = CartRDM.GetInstance(customerID);

            if (cartDatabase.getCart(customerID, out var cartData))
            {
                cartRDM.loadCart(cartData);
                return cartRDM.retrieveProductsInCart();
            }

            // Return an empty cart if no data is found
            return new Dictionary<int, int>();
        }

        // Get product details for the cart
        public Dictionary<int, Dictionary<string, object>> getCartProductDetails(Dictionary<int, int> cart)
        {
            var products = new Dictionary<int, Dictionary<string, object>>();

            foreach (var item in cart)
            {
                var product = productService.getProductDetails(item.Key);
                if (product != null)
                {
                    var productDetails = product.GetProductDetails();
                    productDetails["CostPrice"] = Convert.ToDecimal(productDetails["CostPrice"]); // Ensure CostPrice is decimal
                    products[item.Key] = productDetails;
                }
            }

            return products;
        }

        // Calculate the total cost of the cart
        public decimal calculateCartTotal(Dictionary<int, int> cart, Dictionary<int, Dictionary<string, object>> products)
        {
            return cart.Sum(item => Convert.ToDecimal(products[item.Key]["CostPrice"]) * item.Value);
        }
    }
}