using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IRefundDetails
    {
        Task ReturnItemToInventory(List<int> itemIds, string refundReason);
    }
}