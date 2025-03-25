using System;
using System.Collections.Generic;

namespace CleanBrilliantCompany.Models
{
    public class ReorderRequest_RDM
    {
        public int ReorderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int ManufacturerId { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public string Status { get; set; } = "Pending";
        public int? DefectQuantity { get; set; }
    }

}