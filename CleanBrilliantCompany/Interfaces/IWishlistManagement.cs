using System.Collections.Generic;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IWishlistManagement
    {
        bool addToWishlist(int customerId, int productId);
        bool removeFromWishlist(int customerId, int productId);
        List<int> viewWishlist(int customerId);
    }
}