using System.Collections.Generic;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models
{
    public class WishlistManagement : IWishlistManagement
    {
        private WishlistRDM wishlistRDM = new WishlistRDM();

        public bool AddToWishlist(int productId)
        {
            // Implementation logic here
            return true;
        }

        public bool RemoveFromWishlist(int productId)
        {
            // Implementation logic here
            return false;
        }

        public List<Product> ViewWishlist()
        {
            return wishlistRDM.FetchWishlistDetails();
        }
    }
}