using System.Collections.Generic;

namespace CleanBrilliantCompany.Models
{
    public class WishlistRDM
    {
        private int customerId;
        private Dictionary<Product, int> productsInWishlist = new Dictionary<Product, int>();

        private int GetCustomerId()
        {
            return customerId;
        }

        private void SetCustomerId(int customerId)
        {
            this.customerId = customerId;
        }

        private void SetProductsInWishlist(Product product)
        {
            if (productsInWishlist.ContainsKey(product))
            {
                productsInWishlist[product]++;
            }
            else
            {
                productsInWishlist[product] = 1;
            }
        }

        private List<Product> GetProductsInWishlist()
        {
            return new List<Product>(productsInWishlist.Keys);
        }

        public List<Product> FetchWishlistDetails()
        {
            // Implementation logic here
            return GetProductsInWishlist();
        }
    }
}