using System;
using System.Collections.Generic;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Models
{
    public class RefundManagement : IRefundQuery, ISubmitRefund
    {   
        private readonly IRefundDatabase _refundDatabase;
        private readonly Func<IOrder> _order;
        private readonly IRefundDetails _refundDetails;


        public RefundManagement(IRefundDatabase refundDatabase, Func<IOrder> order, IRefundDetails refundDetails)
        {
            _refundDatabase = refundDatabase;
            _order = order;
            _refundDetails = refundDetails;
        }
        public Refund_RDM GetRefundDetails(int refundId) 
        {
            return _refundDatabase.ViewRefund(refundId);
        }
        
        public List<Refund_RDM> GetAllRefunds()
        {
            return _refundDatabase.GetAllRefunds(); 
        }

        public void UpdateRefund(int refundId, string status)
        {
            var refund = _refundDatabase.ViewRefund(refundId);

            if (refund == null || refund.Status is "Approved" or "Rejected")
            {
                return;
            }

            if (status == "Approved")
            {
                List<int> itemIds = refund.RefundedProducts.Keys.ToList();
                
                _refundDetails.ReturnItemToInventory(itemIds, refund.RefundReason); // Module 2 Team 3's Interface
                Console.WriteLine($"{refund.RefundAmount} has been refunded to the customer in Order {refund.OrderId}.");
            }
            
            _refundDatabase.UpdateRefundStatus(refundId, status, DateTime.Now);

            string orderStatus = status == "Approved" ? "Refunded" : "Rejected";

            var refundDetails = _refundDatabase.ViewRefund(refundId);
            int orderId = refundDetails.OrderId;

            // Resolve IOrder only when needed
            var orderService = _order();           
            orderService.updateOrderStatus(orderId, orderStatus); // Module 1 Team 5's Interface
        }

        public Refund_RDM SubmitRefund(int orderId, string refundReason, float refundAmount, Dictionary<int, int> refundedProducts)
        {
            return _refundDatabase.InsertRefund(orderId, refundReason, refundAmount, refundedProducts);
        }

    }
}
