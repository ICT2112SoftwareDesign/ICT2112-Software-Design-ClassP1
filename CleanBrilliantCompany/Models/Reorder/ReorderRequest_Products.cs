using System;
using System.Collections.Generic;

namespace CleanBrilliantCompany.Models
{
    public class ReorderRequest_Products
    {
        private int reorderProductId;
        private int productId;
        private int quantity;
        private int defectQuantity;

        public int ReorderProductId 
        { 
            get { return reorderProductId; }
            set { reorderProductId = value; }
        }

        public int ProductId 
        { 
            get { return productId; }
            set { productId = value; }
        }

        public int Quantity 
        { 
            get { return quantity; }
            set { quantity = value; }
        }

        public int DefectQuantity 
        { 
            get { return defectQuantity; }
            set { defectQuantity = value; }
        }
    }
}
