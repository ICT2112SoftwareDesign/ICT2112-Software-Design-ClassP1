using System;
using System.Collections.Generic;

namespace CleanBrilliantCompany.Models
{
    public class ReorderRequest_RDM
    {
        private int reorderId;
        private int manufacturerId;
        private DateTime? expectedDeliveryDate;
        private string status = "Pending";
        private List<ReorderRequest_Products> products = new List<ReorderRequest_Products>();

        public int ReorderId 
        { 
            get { return reorderId; } 
            set { reorderId = value; }
        }

        public int ManufacturerId 
        { 
            get { return manufacturerId; } 
            set { manufacturerId = value; }
        }

        public DateTime? ExpectedDeliveryDate 
        { 
            get { return expectedDeliveryDate; } 
            set { expectedDeliveryDate = value; }
        }

        public string Status 
        { 
            get { return status; } 
            set { status = value; }
        }

        public List<ReorderRequest_Products> Products 
        { 
            get { return products; } 
            set { products = value; }
        }
    }
}
