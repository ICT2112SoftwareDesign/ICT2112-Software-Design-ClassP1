using System;

namespace CleanBrilliantCompany.Models
{
    public class OrderRDM
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public string OrderAddress { get; set; }
        public Dictionary<int, int> OrderProducts { get; set; } // Product ID -> Quantity
        public string OrderShipping { get; set; }
        public int OrderItems { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal OrderTotal { get; set; }
    }
}