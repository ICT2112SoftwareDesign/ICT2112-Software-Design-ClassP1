using System.Collections.Generic;

namespace CleanBrilliantCompany.Interfaces
{
    public interface ICartDatabase
    {
        bool addCart(int customerID, Dictionary<int, int> productsInCart);
        bool updateCart(int customerID, Dictionary<int, int> productsInCart);
        bool removeFromCart(int customerID, int productId); // Updated method name
        bool getCart(int customerID, out Dictionary<int, int> productsInCart); 
        bool hasProductInCart(int customerID, int productId);
    }
}