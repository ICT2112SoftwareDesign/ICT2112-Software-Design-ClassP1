using System.Collections.Generic;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Models
{
    public class CartManagement
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

        // Adds an observer to the list
        public void AddObserver(ICartObserver observer)
        {
            observers.Add(observer);
        }

        // Removes an observer from the list
        public void RemoveObserver(ICartObserver observer)
        {
            observers.Remove(observer);
        }

        // Notifies all observers of a change
        private void NotifyObservers()
        {
            foreach (var observer in observers)
            {
                observer.CartUpdated();
            }
        }

         // Adds a product to the cart
        public bool AddToCart(int customerID, int productId, int quantity)
        {
            var productDetails = productService.GetProductDetails(productId);
            if (productDetails != null)
            {
                // Add the product ID and quantity to the in-memory cart (CartRDM)
                cartRDM.AddProduct(productId, quantity);

                // Update the database with the new cart content
                var success = cartDatabase.AddCart(customerID, cartRDM.RetrieveProductsInCart());
                if (success)
                {
                    NotifyObservers();
                }
                return success;
            }
            return false;
        }

         // Updates the quantity of a product in the cart
        public bool UpdateQuantity(int customerID, int productId, int quantity)
        {
          

            // Load the cart from the database into the in-memory cart
            if (cartDatabase.GetCart(customerID, out var cartData))
            {
                cartRDM.LoadCart(cartData);
            }
            else
            {
                return false;
            }

            // Check if the product exists in the in-memory cart
            if (cartRDM.HasProduct(productId))
            {

                // Update the in-memory cart
                cartRDM.UpdateProductQuantity(productId, quantity);

                // Update the database with the new cart content
                var success = cartDatabase.UpdateCart(customerID, cartRDM.RetrieveProductsInCart());
                if (success)
                {
                    NotifyObservers();
                }
                return success;
            }
            return false;
        }

            public bool RemoveFromCart(int customerID, int productId)
        {

            // Load the cart from the database into the in-memory cart
            if (cartDatabase.GetCart(customerID, out var cartData))
            {
                cartRDM.LoadCart(cartData);
            }
            else
            {
                return false;
            }

            // Check if the product exists in the in-memory cart
            if (cartRDM.HasProduct(productId))
            {

                // Remove the product from the in-memory cart
                cartRDM.RemoveProduct(productId);

                // Update the database with the new cart content
                var success = cartDatabase.RemoveFromCart(customerID, productId);
                if (success)
                {
                    NotifyObservers();
                }
                return success;
            }

            return false;
        }

        // Retrieves the cart for a specific customer
        public Dictionary<int, int> ViewCart(int customerID)
        {
            if (cartDatabase.GetCart(customerID, out var cartData))
            {
                cartRDM.LoadCart(cartData);
                return cartRDM.RetrieveProductsInCart();
            }

            // Return an empty cart if no data is found
            return new Dictionary<int, int>();
        }
    }
}