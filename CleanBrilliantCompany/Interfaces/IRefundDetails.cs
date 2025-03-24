using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IRefundDetails
    {
        void ReturnItemToInventory(List<int> itemIds, string refundReason);
    }
}