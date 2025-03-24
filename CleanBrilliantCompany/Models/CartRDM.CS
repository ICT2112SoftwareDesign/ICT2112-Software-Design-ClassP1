using System.Collections.Generic;

namespace CleanBrilliantCompany.Models
{
    // Ensures that each customer has a single, unique instance of their cart.
    public class CartRDM
    {
        // Static dictionary to store a Singleton instance for each customer.
        // Key: Customer ID, Value: CartRDM instance for that customer.
        private static readonly Dictionary<int, CartRDM> customerCarts = new Dictionary<int, CartRDM>();

        // Lock object to ensure thread safety when creating or retrieving instances.
        private static readonly object lockObj = new object();

        // The ID of the customer associated with this cart.
        private int customerId;

        // Dictionary to store the products in the cart.
        // Key: Product ID, Value: Quantity of the product.
        private Dictionary<int, int> productsInCart;

        // Private constructor to prevent direct instantiation
        // Initializes the cart for the specified customer.
        private CartRDM(int customerId)
        {
            this.customerId = customerId;
            productsInCart = new Dictionary<int, int>();
        }

        // Public static method to get the Singleton instance for a specific customer
        // If no instance exists for the customer, a new one is created.
        public static CartRDM GetInstance(int customerId)
        {
            lock (lockObj)
            {
                if (!customerCarts.ContainsKey(customerId))
                {
                    customerCarts[customerId] = new CartRDM(customerId);
                }
                return customerCarts[customerId];
            }
        }

        // Adds a product to the cart 
        public void addProduct(int productId, int quantity)
        {
            if (productsInCart.ContainsKey(productId))
            {
                // If the product already exists, increase its quantity.
                productsInCart[productId] += quantity;
            }
            else
            {   
                // Otherwise, add the product with the specified quantity.
                productsInCart[productId] = quantity;
            }
        }

        // Updates the quantity of a product in the cart.
        public void updateProductQuantity(int productId, int quantity)
        {
            if (productsInCart.ContainsKey(productId))
            {   
                // Update the quantity of the product if it exists in the cart.
                productsInCart[productId] = quantity;
            }
        }

        // Removes a product from the cart.
        public void removeProduct(int productId)
        {
            if (productsInCart.ContainsKey(productId))
            {
                productsInCart.Remove(productId);
            }
        }

        // Public method to check if the cart has a specific product
        public bool hasProduct(int productId)
        {
            return productsInCart.ContainsKey(productId);
        }

        
        // Loads the cart data from the provided dictionary.
        public void loadCart(Dictionary<int, int> cartData)
        {
            // Create a new dictionary copy of the cart data
            productsInCart = new Dictionary<int, int>(cartData);
        }

        // Retrieves the products in the cart.
        public Dictionary<int, int> retrieveProductsInCart()
        {
            // Return a copy of the cart data to prevent external modification.
            return new Dictionary<int, int>(productsInCart);
        }
    }
}