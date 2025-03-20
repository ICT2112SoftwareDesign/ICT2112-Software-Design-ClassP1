using System.Collections.Generic;

namespace CleanBrilliantCompany.Interfaces
{
    public interface ICartManagement
    {
        // Adds a product to the cart
        bool AddToCart(int customerId, int productId, int quantity);

        // Updates the quantity of a product in the cart
        bool UpdateQuantity(int customerId, int productId, int quantity);

        // Removes a product from the cart
        bool RemoveFromCart(int customerId, int productId);
    }
}