using System;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IWishlistQueryObserver
    {
        void LogGetWishlist(int customerId);
        void LogWishlistExists(int customerId, bool exists);
        void LogSaveWishlist(int customerId, string productIdsString, bool success);
    }
}