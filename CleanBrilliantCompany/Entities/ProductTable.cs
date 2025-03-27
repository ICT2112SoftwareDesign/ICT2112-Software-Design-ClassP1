using Microsoft.AspNetCore.Mvc;

namespace CleanBrilliantCompany.Entities
{
    public class ProductTable
    {
        public int productId { get; set; }
        public string productName { get; set; }
        public string productCategory { get; set; }
        public double productCost { get; set; }
        public int manufacturerId { get; set; }
        public double productWeight { get; set; }
        public int quantity { get; set; }
        public int volume { get; set; }
        public double toxicityPercentage { get; set; }
        public int carbonFootprint { get; set; }
        public string productState { get; set; }
    }
}
