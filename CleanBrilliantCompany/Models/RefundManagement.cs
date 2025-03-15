using System;
using System.Collections.Generic;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models {
    public class RefundFactory
    {
        public static Refund_RDM CreateRefund(string type, int orderId, int refundId, string reason, float amount)
        {
            return type switch
            {
                "Standard" => new StandardRefund(orderId, refundId, reason, amount),
                // "Damaged" => new DamagedRefund(),
                // "Partial" => new PartialRefund(),
                _ => throw new ArgumentException("Invalid refund type")
            };
        }
    }

    public class StandardRefund : Refund_RDM
    {
        public StandardRefund(int orderId, int refundId, string reason, float amount)
        {
            OrderId = orderId;
            RefundId = refundId;
            RefundReason = reason;
            RefundAmount = amount;
            RefundRequestDate = DateTime.Now;
            Status = "Pending";
        }
    }

    public class RefundService : IRefundQuery
    {
        private List<Refund_RDM> _refunds = new List<Refund_RDM>();

        public RefundService()
        {
            // fake refund records for temporary use
            _refunds.Add(new Refund_RDM
            {
                OrderId = 1001,
                RefundId = 1,
                // CustomerId = 501,
                RefundReason = "Defective Product",
                RefundAmount = 49.99f,
                RefundRequestDate = DateTime.Now.AddDays(-5),
                RefundProcessedDate = DateTime.Now,
                Status = "Approved",
                RefundedProducts = new Dictionary<int, int> { { 301, 2 } }, // Item ID 301, Quantity 2
                Images = new List<string> { "image1.jpg", "image2.jpg" },
                Videos = new List<string> { "video1.mp4" }
            });

            _refunds.Add(new Refund_RDM
            {
                OrderId = 1002,
                RefundId = 2,
                // CustomerId = 502,
                RefundReason = "Wrong Item Sent",
                RefundAmount = 29.99f,
                RefundRequestDate = DateTime.Now.AddDays(-3),
                Status = "Pending",
                RefundedProducts = new Dictionary<int, int> { { 302, 1 } }, // Item ID 302, Quantity 1
                Images = new List<string>(),
                Videos = new List<string>()
            });

            _refunds.Add(new Refund_RDM
            {
                OrderId = 1003,
                RefundId = 3,
                // CustomerId = 503,
                RefundReason = "Changed Mind",
                RefundAmount = 19.99f,
                RefundRequestDate = DateTime.Now.AddDays(-7),
                RefundProcessedDate = DateTime.Now,
                Status = "Rejected",
                RefundedProducts = new Dictionary<int, int> { { 303, 1 } }, // Item ID 303, Quantity 1
                Images = new List<string> { "refund_proof.jpg" },
                Videos = new List<string>()
            });
        }

        public Refund_RDM GetRefundDetails(int refundId)
        {
            return _refunds.Find(r => r.RefundId == refundId);
        }

        public List<Refund_RDM> GetAllRefunds()
        {
            return _refunds;
        }
    }

}
