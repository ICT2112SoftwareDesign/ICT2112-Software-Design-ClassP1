using System.Collections.Generic;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Models
{
    public class CartManagement :ICartManagement
    {
        private CartRDM cartRDM;
        private ICartDatabase cartDatabase;
        private IProduct productService;
        private List<ICartObserver> observers;

        public CartManagement(ICartDatabase cartDatabase, IProduct productService)
        {
            this.cartDatabase = cartDatabase;
            this.productService = productService;
            cartRDM = new CartRDM();
            observers = new List<ICartObserver>();
        }

        // Adds an observer to the list [NOT IN CLASS DIAGRAM]
        public void addObserver(ICartObserver observer)
        {
            observers.Add(observer);
        }

        // Removes an observer from the list [NOT IN CLASS DIAGRAM]
        public void removeObserver(ICartObserver observer)
        {
            observers.Remove(observer);
        }

        // Notifies all observers of a change [NOT IN CLASS DIAGRAM]
        private void notifyObservers()
        {
            foreach (var observer in observers)
            {
                observer.cartUpdated();
            }
        }

         // Adds a product to the cart [INSIDE CLASS DIAGRAM]
        public bool addToCart(int customerID, int productId, int quantity)
        {
            var productDetails = productService.getProductDetails(productId);
            if (productDetails != null)
            {
                // Add the product ID and quantity to the in-memory cart (CartRDM)
                cartRDM.addProduct(productId, quantity);

                // Update the database with the new cart content
                var success = cartDatabase.addCart(customerID, cartRDM.retrieveProductsInCart());
                if (success)
                {
                    notifyObservers();
                }
                return success;
            }
            return false;
        }

         // Updates the quantity of a product in the cart [INSIDE CLASS DIAGRAM]
        public bool updateQuantity(int customerID, int productId, int quantity)
        {
          

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
                if (success)
                {
                    notifyObservers();
                }
                return success;
            }
            return false;
        }
            // Removes a product from the cart [INSIDE CLASS DIAGRAM]
            public bool removeFromCart(int customerID, int productId) 
        {

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
                if (success)
                {
                    notifyObservers();
                }
                return success;
            }

            return false;
        }

        // Clears the cart after Order is placed [Not in Class Diagram]
        public bool clearCart(int customerID)
        {
            // Call the clearCart method in the cartDatabase (CartMapper)
            var success = cartDatabase.clearCart(customerID);
            if (success)
            {
                // Notify observers that the cart has been cleared
                notifyObservers();
            }
            return success;
        }


        // Retrieves the cart for a specific customer [INSIDE CLASS DIAGRAM]
        public Dictionary<int, int> viewCart(int customerID)
        {
            if (cartDatabase.getCart(customerID, out var cartData))
            {
                cartRDM.loadCart(cartData);
                return cartRDM.retrieveProductsInCart();
            }

            // Return an empty cart if no data is found
            return new Dictionary<int, int>();
        }

        // Get product details for the cart [NOT IN CLASS DIAGRAM]
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

        // Calculate the total cost of the cart [INSIDE CLASS DIAGRAM]
        public decimal calculateCartTotal(Dictionary<int, int> cart, Dictionary<int, Dictionary<string, object>> products)
        {
            return cart.Sum(item => Convert.ToDecimal(products[item.Key]["CostPrice"]) * item.Value);
        }
    }
}