using System.Collections.Generic;

namespace CleanBrilliantCompany.Interfaces
{
   
    // Interface for observing cart-related events.
    public interface ICartQueryObserver
    {
      
        // Triggered when a product is added to the cart.
        void onProductAdded(int customerId, int productId, int quantity);

       
        // Triggered when a product quantity is updated in the cart.
        void onProductQuantityUpdated(int customerId, int productId, int quantity);

        
        // Triggered when a product is removed from the cart.
        void onProductRemoved(int customerId, int productId);

       
        // Triggered when the cart is cleared.
        void onCartCleared(int customerId);

    }
}