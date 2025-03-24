using System.Collections.Generic;

namespace CleanBrilliantCompany.Interfaces
{
    // Interface for cart management operations.
    // Allow other classes to interact with the cart.
    public interface ICartManagement
    {
        // Adds a product to the cart
        bool addToCart(int customerId, int productId, int quantity);

        // Updates the quantity of a product in the cart
        bool updateQuantity(int customerId, int productId, int quantity);

        // Removes a product from the cart
        bool removeFromCart(int customerId, int productId);

         // Retrieves the products in the cart
        Dictionary<int, Dictionary<string, object>> getCartProductDetails(Dictionary<int, int> cart);

        // Calculates the total price of the products in the cart
        decimal calculateCartTotal(Dictionary<int, int> cart, Dictionary<int, Dictionary<string, object>> products);

    }
}