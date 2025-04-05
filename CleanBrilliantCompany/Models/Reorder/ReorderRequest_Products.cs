using System;
using System.Collections.Generic;

namespace CleanBrilliantCompany.Models
{
    public class ReorderRequest_Products
    {
        public int ReorderProductId { get; set; }
        public int ProductId { get; set; } 
        public int Quantity { get; set; } 
        public int? DefectQuantity { get; set; }


    }
}
