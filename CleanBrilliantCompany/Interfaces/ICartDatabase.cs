using System.Collections.Generic;

namespace CleanBrilliantCompany.Interfaces
{   
    // Interface for database operations related to the shopping cart. 
    // Basically allow other classes to interact with the cart database.
    public interface ICartDatabase
    {   
        // Adds a new cart or updates an existing cart for a specific customer.
        bool addCart(int customerID, Dictionary<int, int> productsInCart);

        // Updates the cart for a specific customer with new product quantities.
        bool updateCart(int customerID, Dictionary<int, int> productsInCart);

        // Removes a specific product from a customer's cart.
        bool removeFromCart(int customerID, int productId); 

        // Retrieves the cart for a specific customer.
        bool getCart(int customerID, out Dictionary<int, int> productsInCart); 

        // Checks if a specific product exists in a customer's cart.
        bool hasProductInCart(int customerID, int productId);
        
        // Clears all products from a customer's cart.        
        bool clearCart(int customerID);
    }
}