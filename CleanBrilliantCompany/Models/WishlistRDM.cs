using System.Collections.Generic;

namespace CleanBrilliantCompany.Models
{
    public class WishlistRDM
    {
        private int customerId;
        private List<int> productsInWishlist = new List<int>();

        private int GetCustomerId()
        {
            return customerId;
        }

        private void SetCustomerId(int customerId)
        {
            this.customerId = customerId;
        }

        private void SetProductsInWishlist(List<int> productsInWishlist)
        {
            this.productsInWishlist = productsInWishlist;
        }

        private List<int> GetProductsInWishlist()
        {
            return productsInWishlist;
        }

        public List<int> FetchWishlistDetails()
        {
            // Implementation logic here
            return GetProductsInWishlist();
        }
    }
}