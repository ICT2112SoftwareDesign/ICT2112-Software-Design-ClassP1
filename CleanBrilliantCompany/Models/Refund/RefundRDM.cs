using System;
using System.Collections.Generic;

namespace CleanBrilliantCompany.Models
{
    public class Refund_RDM
    {
        public int OrderId { get; set; }
        public int RefundId { get; set; }
        public Dictionary<int, int> RefundedProducts { get; set; } = new Dictionary<int, int>(); 
        public string RefundReason { get; set; } = string.Empty;
        public float RefundAmount { get; set; }
        public DateTime RefundRequestDate { get; set; }
        public DateTime? RefundProcessedDate { get; set; }
        public string Status { get; set; } = "Pending";
    }
}
