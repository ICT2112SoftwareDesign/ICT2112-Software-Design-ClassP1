using System.Collections.Generic;

namespace CleanBrilliantCompany.Interfaces
{
    public interface ICartDatabase
    {
        bool AddCart(int customerID, Dictionary<int, int> productsInCart);
        bool UpdateCart(int customerID, Dictionary<int, int> productsInCart);
        bool RemoveFromCart(int customerID, int productId); // Updated method name
        bool GetCart(int customerID, out Dictionary<int, int> productsInCart); 
        bool HasProductInCart(int customerID, int productId);
    }
}