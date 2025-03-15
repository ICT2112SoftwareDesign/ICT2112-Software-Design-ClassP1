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
        private int GetCustomerId()
        {
            return customerId;
        }

        // Private setter for customerId
        private void SetCustomerId(int customerId)
        {
            this.customerId = customerId;
        }

        // Private getter for productsInCart
        private Dictionary<int, int> GetProductsInCart()
        {
            return new Dictionary<int, int>(productsInCart);
        }

        // Private setter for productsInCart
        private void SetProductsInCart(Dictionary<int, int> cartProducts)
        {
            productsInCart = new Dictionary<int, int>(cartProducts);
        }

        // Public method to fetch cart details
        public Dictionary<int, int> FetchCartDetails(int customerId)
        {
            if (this.customerId == customerId)
            {
                return GetProductsInCart();
            }
            return new Dictionary<int, int>();
        }

        // Public method to set the customer ID [not in Class Diagram]
        public void AssignCustomerId(int customerId)
        {
            SetCustomerId(customerId);
        }

        // Public method to set the products in the cart [not in Class Diagram]
        public void UpdateProductsInCart(Dictionary<int, int> cartProducts)
        {
            SetProductsInCart(cartProducts);
        }

        // Public method to get the products in the cart [not in class diagram]
        public Dictionary<int, int> RetrieveProductsInCart()
        {
            return GetProductsInCart();
        }

        // Public method to add a product to the cart [not in Class Diagram]
        public void AddProduct(int productId, int quantity)
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
        public void UpdateProductQuantity(int productId, int quantity)
        {
            if (productsInCart.ContainsKey(productId))
            {
                productsInCart[productId] = quantity;
            }
        }

        // Public method to remove a product from the cart [Not in Class Diagram]
        public void RemoveProduct(int productId)
        {
            if (productsInCart.ContainsKey(productId))
            {
                productsInCart.Remove(productId);
            }
        }

        // Public method to check if the cart has a specific product [Not in Class Diagram]
        public bool HasProduct(int productId)
        {
            return productsInCart.ContainsKey(productId);
        }
    }
}

