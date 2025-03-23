using System.Collections.Generic;

namespace CleanBrilliantCompany.Models
{
    public class CartRDM
    {
        // Private attributes 
        private int customerId;

        // Private attribute (Product ID, Quantity) in productsInCart
        private Dictionary<int, int> productsInCart;

        // Constructor
        public CartRDM()
        {
            productsInCart = new Dictionary<int, int>();
        }

        // Private getter for customerId
        private int getCustomerId()
        {
            return customerId;
        }

        // Private setter for customerId
        private void setCustomerId(int customerId)
        {
            this.customerId = customerId;
        }

        // Private getter for productsInCart
        private Dictionary<int, int> getProductsInCart()
        {
            return new Dictionary<int, int>(productsInCart);
        }

        // Private setter for productsInCart
        private void setProductsInCart(Dictionary<int, int> cartProducts)
        {
            productsInCart = new Dictionary<int, int>(cartProducts);
        }

        // Public method to fetch cart details [In class diagram but Not using]
        public Dictionary<int, int> fetchCartDetails(int customerId)
        {
            if (this.customerId == customerId)
            {
                return getProductsInCart();
            }
            return new Dictionary<int, int>();
        }

        // Public method to set the customer ID [not in Class Diagram]
        public void assignCustomerId(int customerId)
        {
            setCustomerId(customerId);
        }

        // Public method to set the products in the cart [not in Class Diagram]
        public void updateProductsInCart(Dictionary<int, int> cartProducts)
        {
            setProductsInCart(cartProducts);
        }

        // Public method to add a product to the cart [not in Class Diagram]
        public void addProduct(int productId, int quantity)
        {
            if (productsInCart.ContainsKey(productId))
            {
                productsInCart[productId] += quantity;
            }
            else
            {
                productsInCart[productId] = quantity;
            }
        }

        // Public method to update the quantity of a product in the cart [Not in Class Diagram]
        public void updateProductQuantity(int productId, int quantity)
        {
            if (productsInCart.ContainsKey(productId))
            {
                productsInCart[productId] = quantity; // Update the quantity for the specific product
            }
        }

        // Public method to remove a product from the cart [Not in Class Diagram]
        public void removeProduct(int productId)
        {
            if (productsInCart.ContainsKey(productId))
            {
                
                productsInCart.Remove(productId);
            }
        }

        // Public method to check if the cart has a specific product [Not in Class Diagram]
        public bool hasProduct(int productId)
        {
            return productsInCart.ContainsKey(productId);
        }

        // Public method to load the cart data [Not in Class Diagram]
        public void loadCart(Dictionary<int, int> cartData)
        {
            productsInCart = new Dictionary<int, int>(cartData);
        }
        
        // Public method to retrieve the products in the cart [Not in Class Diagram]
        public Dictionary<int, int> retrieveProductsInCart()
        {
            return new Dictionary<int, int>(productsInCart);
        }
    }
}