namespace CleanBrilliantCompany.ForecastManagement.DTO
{
    public class ProductDTO
    {
        private int id;
        private string name;
        public int ID { get { return id; } set { id = value; } }
        public string Name { get { return name; } set { name = value; } }

        public ProductDTO(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
