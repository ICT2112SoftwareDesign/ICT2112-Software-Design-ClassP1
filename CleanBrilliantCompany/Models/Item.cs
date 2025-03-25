namespace CleanBrilliantCompany.Models
{
    public class Item
    {
        public int ItemId {get; set;}
        public int ProductId {get; set;}
        public string Name { get; set; }
        public double Weight { get; set; }
        public int Quantity { get; set; }

        public int BatchCode { get; set; }

        public Item(int itemId, int productId, string name, double weight, int quantity, int batchcode)
        {
            ProductId = productId;
            ItemId = itemId;
            Name = name;
            Weight = weight;
            Quantity = quantity;
            BatchCode = batchcode;
        }

        public Item() { }
    }
}
