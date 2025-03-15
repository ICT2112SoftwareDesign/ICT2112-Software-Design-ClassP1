using System.Collections.Generic;

namespace CleanBrilliantCompany.Interfaces
{
    public interface ICartDatabase
    {
        bool AddCart(Dictionary<int, int> productsInCart);
        bool UpdateCart(Dictionary<int, int> productsInCart);
    }
}