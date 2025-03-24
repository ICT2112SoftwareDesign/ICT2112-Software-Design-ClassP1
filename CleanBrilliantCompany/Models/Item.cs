namespace CleanBrilliantCompany.Models
{
    public class Item
    {
        public string Name { get; set; }
        public double Weight { get; set; }
        public int Quantity { get; set; }

        public int BatchCode { get; set; }

        public Item(string name, double weight, int quantity, int batchcode)
        {
            Name = name;
            Weight = weight;
            Quantity = quantity;
            BatchCode = batchcode;
        }

        public Item() { }
    }
}
