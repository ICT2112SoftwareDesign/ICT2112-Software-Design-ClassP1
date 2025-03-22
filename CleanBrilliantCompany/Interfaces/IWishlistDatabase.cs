public interface IWishlistDatabase
{
    string getWishlistProductIdsString(int customerId);
    bool saveWishlistProductIdsString(int customerId, string productIdsString);
    bool customerWishlistExists(int customerId);
}