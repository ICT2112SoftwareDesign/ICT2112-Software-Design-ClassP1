using System.Collections.Generic;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IRefundDetails
    {
        void ReturnItemToInventory(List<int> itemIds, string refundReason);
    }

    public class RefundDetails : IRefundDetails
    {
        public void ReturnItemToInventory(List<int> itemIds, string refundReason)
        {
            // Console.WriteLine("Returning items to inventory...");
            foreach (var itemId in itemIds)
            {
                Console.WriteLine($"Inventory for item {itemId} has been updated. Reason: {refundReason}");
            }
        }
    }
}
