using System.Collections.Generic;

namespace CleanBrilliantCompany.Interfaces
{
    public interface ICartDatabase
    {   
        // INSIDE CLASS DIAGRAM
        bool addCart(int customerID, Dictionary<int, int> productsInCart);
        bool updateCart(int customerID, Dictionary<int, int> productsInCart);

        // NOT IN CLASS DIAGRAM
        bool removeFromCart(int customerID, int productId); 
        bool getCart(int customerID, out Dictionary<int, int> productsInCart); 
        bool hasProductInCart(int customerID, int productId);
        bool clearCart(int customerID);
    }
}