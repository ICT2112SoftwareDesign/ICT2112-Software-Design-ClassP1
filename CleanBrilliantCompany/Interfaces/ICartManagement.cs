using System.Collections.Generic;

namespace CleanBrilliantCompany.Interfaces
{
    public interface ICartManagement
    {
        // Adds a product to the cart
        bool addToCart(int customerId, int productId, int quantity);

        // Updates the quantity of a product in the cart
        bool updateQuantity(int customerId, int productId, int quantity);

        // Removes a product from the cart
        bool removeFromCart(int customerId, int productId);
    }
}