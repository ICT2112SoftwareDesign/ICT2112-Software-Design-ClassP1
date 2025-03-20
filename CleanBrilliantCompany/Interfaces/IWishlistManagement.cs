using System.Collections.Generic;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IWishlistManagement
    {
        bool AddToWishlist(int productId);
        bool RemoveFromWishlist(int productId);
        List<Product> ViewWishlist();
    }
}