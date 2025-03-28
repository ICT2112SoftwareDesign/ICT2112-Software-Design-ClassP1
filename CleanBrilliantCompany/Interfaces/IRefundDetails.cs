using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IRefundDetails
    {
        void returnItemToInventory(List<int> itemIds, string refundReason);
    }
}