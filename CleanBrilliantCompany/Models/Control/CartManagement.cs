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
        public bool AddToCart(int productId, int quantity)
        {
            var productDetails = productService.GetProductDetails(productId);
            if (productDetails != null)
            {
                cartRDM.AddProduct(productId, quantity);
                var success = cartDatabase.AddCart(cartRDM.RetrieveProductsInCart());
                if (success)
                {
                    NotifyObservers();
                }
                return success;
            }
            return false;
        }

        // Updates the quantity of a product in the cart
        public bool UpdateQuantity(int productId, int quantity)
        {
            if (cartRDM.HasProduct(productId))
            {
                cartRDM.UpdateProductQuantity(productId, quantity);
                var success = cartDatabase.UpdateCart(cartRDM.RetrieveProductsInCart());
                if (success)
                {
                    NotifyObservers();
                }
                return success;
            }
            return false;
        }

        // Removes a product from the cart
        public bool RemoveFromCart(int productId)
        {
            if (cartRDM.HasProduct(productId))
            {
                cartRDM.RemoveProduct(productId);
                var success = cartDatabase.UpdateCart(cartRDM.RetrieveProductsInCart());
                if (success)
                {
                    NotifyObservers();
                }
                return success;
            }
            return false;
        }

        // Retrieves the cart details
        public Dictionary<int, int> ViewCart()
        {
            return cartRDM.RetrieveProductsInCart();
        }

        // Calculates the total cost of the items in the cart based on product prices
        public decimal CalculateTotal()
        {
            var products = cartRDM.RetrieveProductsInCart();
            decimal total = 0;
            foreach (var item in products)
            {
                var productDetails = productService.GetProductDetails(item.Key);
                if (productDetails != null)
                {
                    total += (decimal)productDetails.ProductCost * item.Value;
                }
            }
            return total;
        }
    }
}